namespace TechnoSphere_2025.models
{
    public class CharacteristicViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public CharacteristicViewModel(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
