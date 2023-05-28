using System.Collections;

namespace BlazorSplitGrid.Models;

public class Tracks : IEnumerable<Track>
{
    public int Count => _tracks.Count;
    private readonly List<Track> _tracks;

    public Tracks()
    {
        _tracks = new List<Track>();
    }

    public void Add(Track track)
    {
        _tracks.Add(track);
    }

    public bool TryRemove(int track, out Track item)
    {
        if (track < 0 || _tracks.Count <= track || !_tracks[track].IsGutter)
        {
            item = default!;
            return false;
        }

        item = _tracks[track];
        _tracks.RemoveAt(track);
        return true;
    }

    public bool TryRemove(string id, out Track item)
    {
        var track = _tracks.FirstOrDefault(x => x.Id == id && x.IsGutter);
        if (track is null)
        {
            item = default!;
            return false;
        }

        item = track;
        return _tracks.Remove(track);
    }

    public int NextTrack()
    {
        if (_tracks.Count == 0)
            return 0;

        if (_tracks.Last().IsGutter)
            return _tracks.Count;

        return _tracks.Count - 1;
    }

    public bool ShouldAddContent()
    {
        return _tracks.Count == 0 || _tracks.Last().IsGutter;
    }

    public string SetSize(int trackNumber, string? size)
    {
        if (_tracks.Count == 0 || trackNumber < 0 || _tracks.Count <= trackNumber || _tracks[trackNumber].Size == size)
            return "0px";

        var track = _tracks[trackNumber];
        _tracks[trackNumber] = track with { Size = size ?? track.InitialSize };
        return track.Size;
    }

    public string SetSize(string id, string? size)
    {
        for (var i = 0; i < _tracks.Count; i++)
        {
            var track = _tracks[i];
            if (track.Id != id || track.Size == size)
                continue;

            _tracks[i] = track with { Size = size ?? track.InitialSize };
            return track.Size;
        }

        return "0px";
    }

    public string? GetSize(int track)
    {
        if (_tracks.Count == 0 || track < 0 || _tracks.Count <= track)
            return null;

        return _tracks[track].Size;
    }

    public string? GetSize(string id)
    {
        return _tracks.FirstOrDefault(x => x.Id == id)?.Size;
    }

    public bool ResetSizes()
    {
        for (var i = 0; i < _tracks.Count; i++)
        {
            var track = _tracks[i];
            if (track.Size == track.InitialSize)
                continue;

            _tracks[i] = track with { Size = track.InitialSize };
        }

        return true;
    }

    public string BuildTemplate()
    {
        return $"{string.Join(" ", _tracks.Select(x => x.ToSizeString()))}";
    }

    public IEnumerator<Track> GetEnumerator()
    {
        return _tracks.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
