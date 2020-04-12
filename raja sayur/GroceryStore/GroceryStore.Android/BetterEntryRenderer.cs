using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using GroceryStore.Controls;
using Android.Content;
using GroceryStore.Droid;
using Android.Support.V4.Content;
using Android.Text;
using Android.Text.Style;
using AApplication = Android.App.Application;
using Android.Graphics;
using System.Text.RegularExpressions;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(BetterEntry), typeof(BetterEntryRenderer))]
namespace GroceryStore.Droid
{
    public class BetterEntryRenderer : EntryRenderer
    {
        private BetterEntry CustomElement => Element as BetterEntry;
        public BetterEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // UpdatePlaceholderFont();
                Control.SetBackground(ContextCompat.GetDrawable(Context, Android.Resource.Color.Transparent));
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            //if (e.PropertyName == CustomEntry.PlaceholderFontFamilyProperty.PropertyName)
            //    UpdatePlaceholderFont();
        }

        private void UpdatePlaceholderFont()
        {
            if (CustomElement.PlaceholderFontFamily == default(string))
            {
                Control.HintFormatted = null;
                Control.Hint = CustomElement.Placeholder;
                Control.SetHintTextColor(CustomElement.PlaceholderColor.ToAndroid());
                return;
            }

            var placeholderFontSize = (int)CustomElement.FontSize;
            var placeholderSpan = new SpannableString(CustomElement.Placeholder);
            placeholderSpan.SetSpan(new AbsoluteSizeSpan(placeholderFontSize, true), 0, placeholderSpan.Length(), SpanTypes.InclusiveExclusive); // Set Fontsize

            var typeFace = FindFont(CustomElement.PlaceholderFontFamily);
            //var typeFaceSpan = new CustomTypefaceSpan(typeFace);
            //placeholderSpan.SetSpan(typeFaceSpan, 0, placeholderSpan.Length(), SpanTypes.InclusiveExclusive); //Set Fontface

            Control.HintFormatted = placeholderSpan;
        }

        const string LoadFromAssetsRegex = @"\w+\.((ttf)|(otf))\#\w*";
        private Typeface FindFont(string fontFamily)
        {
            if (!string.IsNullOrWhiteSpace(fontFamily))
            {
                if (Regex.IsMatch(fontFamily, LoadFromAssetsRegex))
                {
                    var typeface = Typeface.CreateFromAsset(AApplication.Context.Assets, FontNameToFontFile(fontFamily));
                    return typeface;
                }
                else
                {
                    return Typeface.Create(fontFamily, TypefaceStyle.Normal);
                }
            }

            return Typeface.Create(Typeface.Default, TypefaceStyle.Normal);
        }

        private string FontNameToFontFile(string fontFamily)
        {
            int hashtagIndex = fontFamily.IndexOf('#');
            if (hashtagIndex >= 0)
                return fontFamily.Substring(0, hashtagIndex);

            throw new InvalidOperationException($"Can't parse the {nameof(fontFamily)} {fontFamily}");
        }
    }
}