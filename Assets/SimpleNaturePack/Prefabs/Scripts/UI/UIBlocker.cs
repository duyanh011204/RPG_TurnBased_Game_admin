public static class UIBlocker
{
    public static bool IsAnyPanelOpen = false;

    public static void OpenPanel() { IsAnyPanelOpen = true; }
    public static void ClosePanel() { IsAnyPanelOpen = false; }
}
