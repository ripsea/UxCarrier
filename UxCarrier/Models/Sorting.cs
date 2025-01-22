namespace UxCarrier.Models
{
    public class Sorting
    {
        public IList<SortItem> Items = new List<SortItem>();

        public Sorting(string text)
        {
            var txtItems = String.IsNullOrEmpty(text) ? new string[] { } : text.Split(',');
            foreach (var txtItem in txtItems)
            {
                var item = new SortItem(txtItem);
                if (String.IsNullOrEmpty(item.Key))
                    continue;
                Items.Add(item);
            }
        }

        public bool Has(string itemKey) => Items.Any(i => i.Key == itemKey);

        public bool IsAsc(string itemKey) => Items.Where(i => i.Key == itemKey).SingleOrDefault().IsAsc;
    }

    public class SortItem
    {
        public SortItem(string txtItem)
        {
            if (txtItem.StartsWith("-"))
            {
                IsAsc = false;
                Key = txtItem.Substring(1).Trim();
            }
            else if (txtItem.StartsWith("+"))
            {
                IsAsc = true;
                Key = txtItem.Substring(1).Trim();
            }
            else
            {
                IsAsc = true;
                Key = txtItem.Trim();
            }
        }

        public string Key { get; set; }
        public bool IsAsc { get; set; }
    }
}
