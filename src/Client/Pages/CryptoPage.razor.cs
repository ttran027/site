namespace Client.Pages
{
    public sealed partial class CryptoPage
    {
        private readonly string PanelStyle = "display: flex; flex-direction: column; align-items: center; width: 460px";
        private static RenderFragment GetTitle(string titleText, string href) => builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "d-flex flex-row align-end");
            builder.OpenComponent<IconLink>(2);
            builder.AddAttribute(3, "Icon", Icons.Custom.Brands.GitHub);
            builder.AddAttribute(4, "Size", Size.Small);
            builder.AddAttribute(5, "Href", href);
            builder.AddAttribute(6, "TooltipText", "Click for source code");
            builder.AddAttribute(7, "Style", "color: #ff9800 !important");
            builder.AddAttribute(8, "Class", "mr-2");
            builder.AddAttribute(9, "NewTab", true);
            builder.CloseComponent();
            builder.OpenComponent<MudText>(10);
            builder.AddAttribute(11, "Color", Color.Secondary);
            builder.AddAttribute(12, "Typo", Typo.h6);
            builder.AddAttribute(13, "ChildContent", (RenderFragment)(builder2 =>
            {
                builder2.AddContent(14, titleText);
            }));
            builder.CloseComponent();
            builder.CloseElement();
        };
    }
}