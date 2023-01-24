using bbt.service.notification.ui.Configuration;
using bbt.service.notification.ui.Model;

namespace bbt.service.notification.ui.Override.Service
{
    public class MenuService
    {
        IBaseConfiguration baseConfiguration;
        public MenuService(IBaseConfiguration _baseConfiguration)
        {
            baseConfiguration = _baseConfiguration;
            LoadMenu();
        }
        MenuItem[] allMenuItems;
        private void LoadMenu()
        {
            allMenuItems = new[] {
      

           new MenuItem()
        {
            Name = "Source",
            Path = "/Pages/SourceListPage",
            Icon = "format_list_bulleted",
            Children=null
          
            },
           new MenuItem()
        {
            Name = "Source Log",
            Path = "/Pages/SourceLogListPage",
            Icon = "format_list_bulleted",
            Children=null

            },
             new MenuItem()
        {
            Name = "Message Notification Log",
            Path = "/Pages/LogListPage",
            Icon = "format_list_bulleted",
            Children=null

            },
    };


        }


        public IEnumerable<MenuItem> MenuItems
        {
            get
            {
                return allMenuItems;
            }
        }

        public IEnumerable<MenuItem> Filter(string term)
        {
            Func<string, bool> contains = value => value.Contains(term, StringComparison.OrdinalIgnoreCase);

            Func<MenuItem, bool> filter = (MenuItem) => contains(MenuItem.Name) || (MenuItem.Tags != null && MenuItem.Tags.Any(contains));

            return MenuItems.Where(category => category.Children != null && category.Children.Any(filter))
                           .Select(category => new MenuItem()
                           {
                               Name = category.Name,
                               Expanded = true,
                               Children = category.Children.Where(filter).ToArray()
                           }).ToList();
        }

        public MenuItem FindCurrent(Uri uri)
        {
         
            MenuItem menuItem = MenuItems.SelectMany(MenuItem => MenuItem.Children ?? new[] { MenuItem })
                           .FirstOrDefault(MenuItem => MenuItem.Path == uri.AbsolutePath || $"/{MenuItem.Path}" == uri.AbsolutePath);
            if (menuItem == null)
            {
              //  baseConfiguration.Get<GeneralSettings>().PathBase + "Pages/AnasayfaPage
                int segmentLength = uri.Segments.Length;
                string segment = uri.AbsolutePath.Substring(0, uri.AbsolutePath.IndexOf(uri.Segments[segmentLength - 1]));


            }
            return menuItem;
        }

        public string TitleFor(MenuItem MenuItem)
        {
            if (MenuItem != null && MenuItem.Name != "First Look")
            {
                var MenuItemss = MenuItem.Title ?? $"{MenuItem.Name} |";
                var test = baseConfiguration.Get<GeneralSettings>().Name;
                return MenuItem.Title ?? $"{MenuItem.Name} | " + baseConfiguration.Get<GeneralSettings>().Name;
            }

            return baseConfiguration.Get<GeneralSettings>().Name;
        }

        public string DescriptionFor(MenuItem MenuItem)
        {
            return MenuItem?.Description ?? "BurganBank";
        }
    }
}

