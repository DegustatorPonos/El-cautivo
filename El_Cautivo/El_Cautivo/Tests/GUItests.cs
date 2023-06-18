using El_Cautivo.EngineExtentions;
using NUnit.Framework;
using Microsoft.Xna.Framework;
using Xunit;

namespace El_Cautivo.Tests
{
    public class GUItests
    {
        [Fact]
        public void Button_moves_correctly()
        {
            Button button = new Button(new Vector2(10, 10), Game1.BarrelTexture, null);
            var prevCollider = button.buttonRectangle;

            button.Position = new Vector2(50, 50);
            Assert.AreEqual(button.buttonRectangle.Size, prevCollider.Size);
            Assert.AreNotEqual(button.buttonRectangle.Location, prevCollider.Location);
        }
         //Doesn't matter at all
    }
}
