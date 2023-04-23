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
        public Recipes()
        {
            InitializeComponent();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            string userInput = inputSearch.Text.Replace(" ", ",+");
            string apiKey = "&apiKey=22d37f4ddb3649d1aa1ee372e930b27c";

            string apiUrl = "https://api.spoonacular.com/recipes/findByIngredients?ingredients=" + userInput + apiKey;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    List<Recipe> recipeList = await response.Content.ReadAsAsync<List<Recipe>>();

                    DisplayRecipes(recipeList);
                }
                else
                {
                    MessageBox.Show("Error: Could not retrieve search results");
                }
            }
        }

        private void DisplayRecipes(List<Recipe> recipeList)
        {
            flowLayout.Controls.Clear();

            foreach (Recipe recipe in recipeList)
            {
                RecipeCard card = new RecipeCard(recipe);
                flowLayout.Controls.Add(card);
            }
        }

        public class Ingredient
        {
            public string name { get; set; }
            public string original { get; set; }
        }

        public class Recipe
        {
            public string title { get; set; }
            public string image { get; set; }
            public string Description { get; set; }
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

    }
}
