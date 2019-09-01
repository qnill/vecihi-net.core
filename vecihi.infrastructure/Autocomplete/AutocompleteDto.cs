namespace vecihi.infrastructure
{
    public class AutocompleteDto<Type>
        where Type : struct
    {
        public Type? Id { get; set; }
        public string Text { get; set; }
    }
}
