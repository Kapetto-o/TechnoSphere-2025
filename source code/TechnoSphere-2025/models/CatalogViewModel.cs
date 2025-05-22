using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Configuration;

namespace TechnoSphere_2025.models
{
    public class CatalogViewModel
    {
        public ObservableCollection<CategoryViewModel> Categories { get; }
            = new ObservableCollection<CategoryViewModel>();

        public CatalogViewModel()
        {
            List<Category> flat;
            try
            {
                flat = LoadAllCategoriesFromDb()
                    .Where(c => c.IsActive)
                    .ToList();
            }
            catch (SqlException)
            {
                flat = new List<Category>();
            }
            catch (ConfigurationErrorsException)
            {
                flat = new List<Category>();
            }
            catch (Exception)
            {
                flat = new List<Category>();
            }

            if (!flat.Any())
                return;

            var lookup = flat
                .ToDictionary(c => c.CategoryID, c => new CategoryViewModel(c));

            foreach (var vm in lookup.Values)
            {
                var model = flat.First(c => c.CategoryID == vm.CategoryID);
                if (model.ParentCategoryID.HasValue
                    && lookup.TryGetValue(model.ParentCategoryID.Value, out var parentVm))
                {
                    parentVm.Children.Add(vm);
                }
                else
                {
                    Categories.Add(vm);
                }
            }

            void SortRec(ObservableCollection<CategoryViewModel> list)
            {
                var ordered = list
                    .OrderBy(x => flat.First(c => c.CategoryID == x.CategoryID).SortOrder)
                    .ToArray();
                list.Clear();
                foreach (var item in ordered) list.Add(item);
                foreach (var item in list)
                    SortRec(item.Children);
            }
            SortRec(Categories);
        }

        private Dictionary<int, CategoryViewModel>? _lookup;

        private void EnsureLookup()
        {
            if (_lookup != null) return;
            _lookup = new Dictionary<int, CategoryViewModel>();
            void Walk(IEnumerable<CategoryViewModel> list)
            {
                foreach (var vm in list)
                {
                    _lookup[vm.CategoryID] = vm;
                    Walk(vm.Children);
                }
            }
            Walk(Categories);
        }

        public string GetDisplayName(int categoryId)
        {
            EnsureLookup();
            return _lookup!.TryGetValue(categoryId, out var vm)
                 ? vm.DisplayName
                 : string.Empty;
        }

        private List<Category> LoadAllCategoriesFromDb()
        {
            var result = new List<Category>();
            var connStr = ConfigurationManager
                .ConnectionStrings["TechnoSphereBD"]
                ?.ConnectionString
                ?? throw new ConfigurationErrorsException("Connection string 'TechnoSphereBD' not found.");

            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    @"SELECT CategoryID, ParentCategoryID, Name_Ru, Name_Eng, SortOrder, IsActive
                      FROM Categories", conn))
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        result.Add(new Category
                        {
                            CategoryID = rdr.GetInt32(0),
                            ParentCategoryID = rdr.IsDBNull(1) ? null : (int?)rdr.GetInt32(1),
                            Name_Ru = rdr.GetString(2),
                            Name_Eng = rdr.GetString(3),
                            SortOrder = rdr.GetInt32(4),
                            IsActive = rdr.GetBoolean(5)
                        });
                    }
                }
            }
            return result;
        }
    }
}