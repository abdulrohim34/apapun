using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GroceryStore.Controls
{
    public class UnderlineEffect : RoutingEffect
    {
        public const string EffectNamespace = "Example";

        public UnderlineEffect() : base($"{EffectNamespace}.{nameof(UnderlineEffect)}")
        {
        }
    }
}
