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
using static WindowsFormsApp1.Recipes;

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

            appManager.fetchRecipes(userInput);
        }

        public class RecipeManager
        {
            private List<Recipe> recipeList;
            private List<Recipe> savedRecipeList;
            private ApiManager api;
            private AppConfig config;
            private FlowLayoutPanel flowLayout;

            public RecipeManager(FlowLayoutPanel flowLayout)
            {
                config = new AppConfig();
                api = new ApiManager(config.ApiKey, config.ApiURL);
                this.flowLayout = flowLayout;
            }

            public async void fetchRecipes(string inputString)
            {
                string query = inputString.Replace(" ", ",+");
                recipeList = await api.GetRecipesByIngredientsAsync(query);
                RenderRecipes(recipeList);
            }
            private void RenderRecipes(List<Recipe> recipeList)
            {
                flowLayout.Controls.Clear();

                foreach (Recipe recipe in recipeList)
                {
                    RecipeCard card = new RecipeCard(recipe);
                    flowLayout.Controls.Add(card);
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

            public RecipeCard(Recipe recipe)
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
                    ingredientLabel.Text = ingredient.name;// + " (" + ingredient.original + ")";
                    ingredientLabel.ForeColor = Color.Red;
                    ingredientLabel.AutoSize = true;
                    ingredientsPanel.Controls.Add(ingredientLabel);
                }

                foreach (Ingredient ingredient in recipe.usedIngredients)
                {
                    Label ingredientLabel = new Label();
                    ingredientLabel.Text = ingredient.name;// + " (" + ingredient.original + ")";
                    ingredientLabel.ForeColor = Color.Green;
                    ingredientLabel.AutoSize = true;
                    ingredientsPanel.Controls.Add(ingredientLabel);
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
            private string savedRecipesPath = "./storredRecipes.json";

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
}
