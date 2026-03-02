using Godot;
using Godot.Collections;
using Scripts.InventorySystem;

namespace Scripts.CraftingSystem;
public partial class CraftingSystem : Node
{
    public static CraftingSystem Instance { get; private set; }

    // Recipes
    [Export]
    private Array<Recipe> Recipes = [];

    public override void _Ready()
    {
        Instance = this;
    }

    public void Craft(string recipeId, string itemId)
    {
        var inventory = InventorySystem.InventorySystem.Instance;
        var currentRecipe = FindRecipe(recipeId);
        var ingredient = inventory.GetItem(itemId);
        if (ingredient?.ItemData.Id != currentRecipe.Input.ItemData?.Id) return;

        if (ingredient.Amount < currentRecipe.Input.Amount)
        {
            GD.Print("Not enough resources.");
            return;       
        }
        

        if (!inventory.ToolExists(currentRecipe.Output.Id))
        {
            inventory.RemoveItem(itemId, currentRecipe.Input.Amount);
            inventory.AddTool(currentRecipe.Output);
        }
        else
        {
            GD.Print("Tool already exists in inventory.");
        }
    }

    // Every recipe ends with "_recipe" in the ID
    public Recipe FindRecipe(string id)
    {
        foreach (Recipe recipe in Recipes)
        {
            if (recipe.Id == id)
            {
                return recipe;
            }
        }
        return null;
    }

}
