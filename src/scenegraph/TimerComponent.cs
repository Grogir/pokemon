using System;
using System.Numerics;

public class TimerComponent : TextComponent {

    public ulong StartTime;
    public bool Running = true;
    GameBoy Gb;

    public Vector4 RunningColor = new Vector4(62.0f / 255.0f, 208.0f / 255.0f, 95.0f / 255.0f, 255.0f);
    public Vector4 FinishedColor = new Vector4(74.0f / 255.0f, 173.0f / 255.0f, 241.0f / 255.0f, 255.0f);

    public TimerComponent(float x, float y, float scale = 1) : base("", x, y, scale) {
    }

    public override void OnInit(GameBoy gb) {
        StartTime = gb.EmulatedSamples;
        Gb = gb;
    }

    public void Start() {
        if(Gb != null) OnInit(Gb);
        Running = true;
    }

    public void Stop() {
        Running = false;
    }

    public TimeSpan Duration() {
        return TimeSpan.FromSeconds((Gb.EmulatedSamples - StartTime) / 2097152.0);
    }

    public override void BeginScene(GameBoy gb) {
        TimeSpan duration = Duration();
        if(Running) {
            if(duration.Hours > 0)
                Text = string.Format("{0:h\\:mm\\:ss\\.ff}", duration);
            else if(duration.Minutes > 0)
                Text = string.Format("{0:m\\:ss\\.fff}", duration);
            else
                Text = string.Format("{0:s\\.fff}", duration);
            // if(Text.Length < 10)
            //     Text = new string(' ', 10 - Text.Length) + Text;
        }
    }

    public override void Render(GameBoy gb) {
        int x = (int) (160 - Renderer.Font.CharacterSize * Text.Length * Scale) / 2;
        Renderer.DrawString(Text, X + x, Y, RenderLayer, Scale, Running ? RunningColor : FinishedColor);
    }
}