namespace vecihi.infrastructure
{
    public class Autocomplete<Type>
      where Type : struct
    {
        public Type? Id { get; set; }
        public string Search { get; set; }
        public string Text { get; set; }
    }
}
