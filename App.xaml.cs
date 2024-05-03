namespace AppTime;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }
    protected override Window CreateWindow(IActivationState activationState)
    {
        var displayInfo = DeviceDisplay.MainDisplayInfo;

        double windowWidth = 250;
        double windowHeight = 300;

        double x = displayInfo.Width / displayInfo.Density - windowWidth;
        double y = displayInfo.Height / displayInfo.Density - windowHeight;

        var mainWindow = new Window(new MainPage())
        {
            Width = windowWidth,
            Height = windowHeight,
            X = x,
            Y = 0
        };

        return mainWindow;
    }
}
