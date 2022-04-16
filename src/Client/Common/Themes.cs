namespace Client.Common;

public static class Themes
{
    public static readonly MudTheme DarkTheme = new()
    {
        Palette = new Palette()
        {
            Black = "#27272f",
            Background = AppColors.DarkGrey,
            BackgroundGrey = "#27272f",
            Surface = AppColors.Blue800,
            DrawerBackground = AppColors.Blue800,
            DrawerText = AppColors.Blue300,
            DrawerIcon = AppColors.Blue300,
            AppbarBackground = AppColors.Blue800,
            AppbarText = AppColors.Blue300,
            TextPrimary = AppColors.Blue50,
            TextSecondary = AppColors.Blue300,
            Primary = AppColors.Primary,
            Secondary = AppColors.Secondary,
            ActionDefault = "#adadb1",
            ActionDisabled = "rgba(255,255,255, 0.26)",
            ActionDisabledBackground = "rgba(255,255,255, 0.12)",
            Divider = "rgba(255,255,255, 0.12)",
            DividerLight = "rgba(255,255,255, 0.06)",
            TableLines = "rgba(255,255,255, 0.12)",
            LinesDefault = "rgba(255,255,255, 0.12)",
            LinesInputs = "rgba(255,255,255, 0.3)",
            TextDisabled = "rgba(255,255,255, 0.2)"
        }
    };
}