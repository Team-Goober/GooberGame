using Sprint.Interfaces;
using Sprint.Music.Sfx;
using Sprint.Music.Songs;

namespace Sprint.Functions.Music;

public class MusicDown : ICommand
{
    private SfxFactory sfxFactory;
    private SongHandler songHandler;

    public MusicDown()
    {
        sfxFactory = SfxFactory.GetInstance();
        songHandler = SongHandler.GetInstance();
    }

    public void Execute()
    {
        sfxFactory.VolumeDown();
        songHandler.VolumeDown();
    }
}