using Plugin.Maui.Audio;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace AppTime;

public partial class MainPage : ContentPage
{
    TimeSpan StudyTime = new();
    bool isPlaying;
    TimeSpan tempoTotal = new();

    public MainPage()
    {
        InitializeComponent();


        ItemSelect.SelectedIndex = 3;
        DisplayTime.Text = ItemSelect.Items[ItemSelect.SelectedIndex];
    }

    void OnCounterClicked(object sender, EventArgs e)
    {
        if (ButtonPlay.Text == "⏸️")
            isPlaying = false;
        else
            isPlaying = true;

        Play();
    }

    async void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        isPlaying = false;
        ButtonPlay.Text = "▶️";

        var picker = (Picker)sender;
        int selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            await Task.Delay(500);
            DisplayTime.Text = picker.Items[selectedIndex];
        }
    }

    async void Play()
    {

        if (isPlaying)
        {
            tempoTotal = FormatTime(DisplayTime.Text);
            ButtonPlay.Text = ButtonPlay.Text == "Descanso" ? "Descanso" : "⏸️";
        }
        else
        {
            ButtonPlay.Text = "▶️";
        }
  
        while (isPlaying && tempoTotal.TotalSeconds > 0)
        {
            
            tempoTotal = tempoTotal.Subtract(TimeSpan.FromSeconds(1));
            DisplayTime.Text = tempoTotal.ToString(@"mm\:ss");
            await Task.Delay(1000);
        }
        ToChange();
    }

    void ToChange()
    {
        if (ButtonPlay.Text == "Descanso")
        {
            ResetDefoult();
            RestMode();

            return;
        }
        else if (ButtonPlay.Text == "▶️") return;

        ButtonPlay.Text = "Descanso";
        DisplayTime.Text = "10:00";
        isPlaying = true;
        PageBackgroundColor.BackgroundColor = Colors.Blue;
        Alert();
        FocusMode();
    }

    async void RestMode()
    {
        string tempoSelecionado = ItemSelect.Items[ItemSelect.SelectedIndex].ToString();

        StudyTime += FormatTime(tempoSelecionado);
        Study.Text = StudyTime.ToString(@"hh\:mm\:ss");

        var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("myaudio2.mp3"));
        audioPlayer.Play();
    }

    TimeSpan FormatTime(string tempoSelecionado)
    {
        string[] times = tempoSelecionado.Split(':');
        int minutos = Convert.ToInt32(times[0]);
        int segundos = Convert.ToInt32(times[1]);

        return TimeSpan.FromMinutes(minutos) + TimeSpan.FromSeconds(segundos);
    }

    async void FocusMode()
    {
        var audioPlayer = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("myaudio.mp3"));
        audioPlayer.Play();
        Play();
    }
    void Reset(object sender, EventArgs e)
    {
        ResetDefoult();
    }
    void ResetDefoult()
    {
        ButtonPlay.Text = "▶️";
        DisplayTime.Text = ItemSelect.Items[ItemSelect.SelectedIndex].ToString();
        isPlaying = false;
        PageBackgroundColor.BackgroundColor = Colors.Green;


    }
    async void Alert()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        string text = "Modo concentração acabou, Descanse.";
        ToastDuration duration = ToastDuration.Short;
        double fontSize = 14;

        var toast = Toast.Make(text, duration, fontSize);

        await toast.Show(cancellationTokenSource.Token);
    }
}

