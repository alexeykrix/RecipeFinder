using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static WindowsFormsApp1.Recipes;
using System.Threading;

namespace WindowsFormsApp1
{
    public partial class Recipes : Form
    {
        public RecipeManager appManager;
        public Recipes()
        {
            InitializeComponent();
            appManager = new RecipeManager(flowLayout);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string userInput = inputSearch.Text;
            appManager.SearchMode();
            appManager.fetchRecipes(userInput);
        }

        private void buttonToSaved_Click(object sender, EventArgs e)
        {
            appManager.SavedMode();
        }
    }


    public class RecipeManager
    {
        private List<Recipe> recipeList;
        private List<Recipe> savedRecipeList;
        private ApiManager api;
        private FileManager fileManager;
        private AppConfig config;
        private FlowLayoutPanel flowLayout;
        private Boolean isSavedMode = false;

        public RecipeManager(FlowLayoutPanel flowLayout)
        {
            config = new AppConfig();
            api = new ApiManager(config.ApiKey, config.ApiURL);
            fileManager = new FileManager(config.SavedRecipesPath);
            this.flowLayout = flowLayout;
        }

        public void SavedMode()
        {
            isSavedMode = true;
            savedRecipeList = fileManager.GetSavedRecipes();
            RenderRecipes(savedRecipeList, this);
        }
        public void SearchMode()
        {
            isSavedMode = false;
        }

        public void btnSave_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            int id = (int)clickedButton.Tag;
            this.saveRecipeById(id);

        }
        public void btnRemove_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            int id = (int)clickedButton.Tag;
            this.removeRecipeById(id);
            RenderRecipes(savedRecipeList, this);
        }
        public async void fetchRecipes(string inputString)
        {
            string query = inputString.Replace(" ", ",+");
            recipeList = await api.GetRecipesByIngredientsAsync(query);
            RenderRecipes(recipeList, this);
        }

        public void saveRecipeById(int id)
        {
            Recipe recipe = recipeList.Find(r => r.id == id);
            fileManager.addRecipe(recipe);
        }
        public void removeRecipeById(int id)
        {
            fileManager.DeleteRecipe(id);
        }

        private void RenderRecipes(List<Recipe> recipeList, RecipeManager app)
        {
            flowLayout.Controls.Clear();

            foreach (Recipe recipe in recipeList)
            {
                RecipeCard card = new RecipeCard(recipe, app, isSavedMode);
                flowLayout.Controls.Add(card);
            }
        }

    }

    public class FileManager
    {
        private string SavedRecipesFilePath { get; set; }
        private List<Recipe> recipes = new List<Recipe>();

        public FileManager(string savedRecipesFilePath)
        {
            SavedRecipesFilePath = savedRecipesFilePath;
        }

        private string GetSavedRecipesFilePath()
        {
            string recipesFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            return Path.Combine(recipesFolderPath, SavedRecipesFilePath);
        }


        public List<Recipe> GetSavedRecipes()
        {
            List<Recipe> savedRecipes;
            if (File.Exists(GetSavedRecipesFilePath()))
            {
                string json = File.ReadAllText(GetSavedRecipesFilePath());
                savedRecipes = JsonConvert.DeserializeObject<List<Recipe>>(json);
            }
            else
            {
                savedRecipes = new List<Recipe>();
            }

            return savedRecipes;
        }

        private void SaveRecipes(List<Recipe> recipesList)
        {
            string json = JsonConvert.SerializeObject(recipesList);
            File.WriteAllText(GetSavedRecipesFilePath(), json);
        }

        public void addRecipe(Recipe recipe)
        {
            if (recipes.Count == 0)
            {
                recipes = GetSavedRecipes();
            }

            recipes.Add(recipe);
            SaveRecipes(recipes);
        }

        public async void DeleteRecipe(int recipeId)
        {
            if (recipes.Count == 0)
            {
                recipes = GetSavedRecipes();
            }
            Recipe recipeToRemove = recipes.FirstOrDefault(r => r.id == recipeId);
            if (recipeToRemove != null)
            {
                recipes.Remove(recipeToRemove);
                SaveRecipes(recipes);
            }
        }
    }


    public class ApiManager
    {
        private string ApiKey { get; set; }
        private string ApiURL { get; set; }
        private HttpClient httpClient;

        public ApiManager(string apiKey, string apiURL)
        {
            ApiKey = apiKey;
            ApiURL = apiURL;
            httpClient = new HttpClient();
        }

        public async Task<List<Recipe>> GetRecipesByIngredientsAsync(string query)
        {
            string requestUrl = $"{ApiURL}?apiKey={ApiKey}&ingredients={query}";

            HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            List<Recipe> recipes = JsonConvert.DeserializeObject<List<Recipe>>(responseBody);

            return recipes;
        }

    }

    public class Ingredient
    {
        public string name { get; set; }
        public string original { get; set; }
    }

    public class Recipe
    {
        public int id { get; set; }
        public string title { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public List<Ingredient> missedIngredients { get; set; }
        public List<Ingredient> usedIngredients { get; set; }
    }

    public class RecipeCard : UserControl
    {
        private PictureBox pictureBox;
        private Label nameLabel;
        private FlowLayoutPanel ingredientsPanel;
        private Button buttonSave;
        private Button buttonRemove;

        public RecipeCard(Recipe recipe, RecipeManager app, Boolean isSavedMode)
        {
            pictureBox = new PictureBox();
            pictureBox.Size = new Size(200, 200);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.LoadAsync(recipe.image);

            nameLabel = new Label();
            nameLabel.Text = recipe.title;
            nameLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            nameLabel.AutoSize = true;

            ingredientsPanel = new FlowLayoutPanel();
            ingredientsPanel.FlowDirection = FlowDirection.TopDown;
            ingredientsPanel.AutoSize = true;
            ingredientsPanel.Margin = new Padding(200, 10, 0, 0);
            ingredientsPanel.Location = new Point(200, 0);

            ingredientsPanel.Controls.Add(nameLabel);

            foreach (Ingredient ingredient in recipe.missedIngredients)
            {
                Label ingredientLabel = new Label();
                ingredientLabel.Text = ingredient.name;
                ingredientLabel.ForeColor = Color.Red;
                ingredientLabel.AutoSize = true;
                ingredientsPanel.Controls.Add(ingredientLabel);
            }

            foreach (Ingredient ingredient in recipe.usedIngredients)
            {
                Label ingredientLabel = new Label();
                ingredientLabel.Text = ingredient.name;
                ingredientLabel.ForeColor = Color.Green;
                ingredientLabel.AutoSize = true;
                ingredientsPanel.Controls.Add(ingredientLabel);
            }

            if (!isSavedMode)
            {
                buttonSave = new Button();
                buttonSave.Text = "Save";
                buttonSave.Tag = recipe.id;
                buttonSave.Click += app.btnSave_Click;
                ingredientsPanel.Controls.Add(buttonSave);
            }
            else
            {
                buttonRemove = new Button();
                buttonRemove.Text = "Remove";
                buttonRemove.Tag = recipe.id;
                buttonRemove.Click += app.btnRemove_Click;
                ingredientsPanel.Controls.Add(buttonRemove);
            }


            this.Controls.Add(pictureBox);
            this.Controls.Add(ingredientsPanel);
            this.AutoSize = true;
            this.Margin = new Padding(10);
            this.BorderStyle = BorderStyle.FixedSingle;
        }
    }

    public class AppConfig
    {
        private string apiURL = "https://api.spoonacular.com/recipes/findByIngredients";
        private string apiKey = "22d37f4ddb3649d1aa1ee372e930b27c";
        private string savedRecipesPath = "savedRecipes.json";

        public string ApiKey
        {
            get { return apiKey; }
        }
        public string SavedRecipesPath
        {
            get { return savedRecipesPath; }
        }
        public string ApiURL
        {
            get { return apiURL; }
        }
    }



}
