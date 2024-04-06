using Sprint.Interfaces;
using Sprint.Music.Sfx;
using Sprint.Music.Songs;

namespace Sprint.Functions.Music;

public class MusicMuteToggle : ICommand
{
    private SfxFactory sfxFactory;
    private SongHandler songHandler;

    public MusicMuteToggle()
    {
        sfxFactory = SfxFactory.GetInstance();
        songHandler = SongHandler.GetInstance();
    }

    public void Execute()
    {
        sfxFactory.MuteToggle();
        songHandler.MuteToggle();
    }
}