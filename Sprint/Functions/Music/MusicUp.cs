using Sprint.Interfaces;
using Sprint.Music.Sfx;
using Sprint.Music.Songs;

namespace Sprint.Functions.Music;

public class MusicUp : ICommand
{
    private SfxFactory sfxFactory;
    private SongHandler songHandler;

    public MusicUp()
    {
        sfxFactory = SfxFactory.GetInstance();
        songHandler = SongHandler.GetInstance();
    }

    public void Execute()
    {
        sfxFactory.VolumeUp();
        songHandler.VolumeUp();
    }
}