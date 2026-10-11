using System;
using System.Collections.Generic;

namespace GFC.Core.Models
{
    public class LiquorMenuModel
    {
        // Header & Contact Copy
        public string IntroText { get; set; } = "Below is our current selection of beverages and bar items available for your function.";
        public string SubIntroText { get; set; } = "Looking for something specific that isn’t listed? Email us at";
        public string ContactEmail { get; set; } = "hallrental@gloucesterfraternityclub.com";
        public string ContactEmailNote { get; set; } = "with your request, and our team will get in touch with you to see what we can arrange.";

        // Beverage Categories
        public List<LiquorCategoryItem> Categories { get; set; } = new();

        public static LiquorMenuModel GetDefault()
        {
            return new LiquorMenuModel
            {
                Categories = new List<LiquorCategoryItem>
                {
                    new LiquorCategoryItem
                    {
                        Name = "Beer & Malt",
                        Items = new List<string>
                        {
                            "Budweiser",
                            "Bud Light",
                            "Michelob Light",
                            "Michelob Ultra",
                            "Miller Lite",
                            "Coors Light",
                            "Pabst Blue Ribbon",
                            "Heineken",
                            "Corona",
                            "Sam Adams Light",
                            "Sam Adams Summer",
                            "Twisted Tea",
                            "O’Doul’s"
                        }
                    },
                    new LiquorCategoryItem
                    {
                        Name = "Whiskey & Scotch",
                        Items = new List<string>
                        {
                            "Jameson Irish Whiskey",
                            "Crown Royal",
                            "Jack Daniel’s",
                            "Seagram’s 7",
                            "Seagram’s VO",
                            "Canadian LTD",
                            "Southern Comfort",
                            "Dewar’s White Label",
                            "Glenlivet Single Malt"
                        }
                    },
                    new LiquorCategoryItem
                    {
                        Name = "Vodka, Gin & Tequila",
                        Items = new List<string>
                        {
                            "Tito’s Handmade Vodka",
                            "Absolut",
                            "Smirnoff",
                            "Smirnoff Vanilla",
                            "Stoli Razberi",
                            "Tanqueray Gin",
                            "Jose Cuervo Silver Tequila"
                        }
                    },
                    new LiquorCategoryItem
                    {
                        Name = "Rum",
                        Items = new List<string>
                        {
                            "Bacardi Superior",
                            "Captain Morgan Spiced",
                            "Captain Morgan Parrot Bay",
                            "Sailor Jerry Spiced",
                            "Malibu Coconut"
                        }
                    },
                    new LiquorCategoryItem
                    {
                        Name = "Wine & Vermouth",
                        Items = new List<string>
                        {
                            "Crane Lake Pinot Grigio",
                            "Crane Lake Merlot",
                            "Crane Lake Sparkling Wine",
                            "Chardonnay (8 oz)",
                            "White Zinfandel (8 oz)",
                            "Stock Dry Vermouth",
                            "Stock Sweet Vermouth"
                        }
                    },
                    new LiquorCategoryItem
                    {
                        Name = "Liqueurs & Cordials",
                        Items = new List<string>
                        {
                            "Bailey’s Irish Cream",
                            "Kahlúa",
                            "Sambuca",
                            "Grand Marnier",
                            "Amaretto",
                            "Jägermeister",
                            "Midori Melon",
                            "B&B",
                            "Dr. McGillicuddy's",
                            "1920",
                            "Blackberry, Apricot & Coffee Brandy",
                            "Butterscotch, Peppermint, Peach & Apple Schnapps"
                        }
                    }
                }
            };
        }
    }

    public class LiquorCategoryItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; } = string.Empty;
        public List<string> Items { get; set; } = new();
    }
}
