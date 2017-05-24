using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class RubanPlayer : MonoBehaviour, MovingPlatform
{
    public List<RubanPlaylist> playlists = new List<RubanPlaylist>();

    public float playSpeed = 1;
    public float stopMinSpeed = 3;
    public float timeScale;

    private double heightAnchor;
    private bool isStopped = false;
    private double targetStopDistance;
    private const double ANCHORCAP = -999.99;

    private RubanPlaylist[] activePlaylists = new RubanPlaylist[2];
    private int activePlaylistCount = 0;

    private List<RubanPlaylist> queue = new List<RubanPlaylist>();

    void FixedUpdate()
    {
        if (playSpeed == 0 || isStopped)
            return;

        //Anchor move
        heightAnchor -= Time.fixedDeltaTime * playSpeed * timeScale;
        if (heightAnchor < ANCHORCAP)
            ResetHeightAnchor();

        //Apply movement
        for (int i = 0; i < activePlaylistCount; i++)
        {
            activePlaylists[i].UpdatePos(heightAnchor);
        }
    }

    #region Public

    public void SetPlaySpeed()
    {
        if (isStopped)
            return;
    }

    public void UnStop()
    {
        isStopped = false;
    }

    public void Stop()
    {
        if (isStopped)
            return;
        playSpeed = 0;
        isStopped = true;
    }

    //public void Stop(float remainingDistance)
    //{
    //    if (isStopping)
    //        return;
    //    if (playSpeed < stopMinSpeed)
    //        playSpeed = stopMinSpeed;

    //    targetStopDistance = heightAnchor - remainingDistance;
    //}

    public string CurrentPlaylistName()
    {
        if (activePlaylistCount != 0)
            return activePlaylists[activePlaylistCount].name;
        else
            return "";
    }

    public bool IsActive(string playlistName)
    {
        for (int i = 0; i < activePlaylistCount; i++)
        {
            if (activePlaylists[i].name == playlistName)
                return true;
        }
        return false;
    }

    public bool IsTransitionning()
    {
        return activePlaylistCount > 1;
    }

    public bool HasPlaylist(string name)
    {
        for (int i = 0; i < playlists.Count; i++)
        {
            if (playlists[i].name == name)
                return true;
        }
        return false;
    }

    public int IndexOf(string name)
    {
        for (int i = 0; i < playlists.Count; i++)
        {
            if (playlists[i].name == name)
                return i;
        }
        return -1;
    }

    public float GetVerticalSpeed()
    {
        return -playSpeed * timeScale;
    }

    #endregion

    #region Mode Start

    public void StartNewPlaylist(string name)
    {
        StartNewPlaylist(GetPlaylistByName(name));
    }

    private void StartNewPlaylist(RubanPlaylist playlist)
    {
        CheckIfActive(playlist);

        for (int i = 0; i < activePlaylistCount; i++)
        {
            activePlaylists[i].EndImmediately();
            activePlaylists[i].onBoutDuRouleau = null;
            activePlaylists[i] = null;
        }
        activePlaylistCount = 0;

        queue.Clear();

        activePlaylists[0] = playlist;
        activePlaylistCount++;

        //heightAnchor = 0;
        activePlaylists[0].deltaHeightAnchor = -heightAnchor;
        activePlaylists[0].StartAt(0);
        activePlaylists[0].shouldLoop = true;
    }

    #endregion

    #region Mode Transition

    public void TransitionToNewPlaylist(string name)
    {
        TransitionToNewPlaylist(GetPlaylistByName(name), true);
    }

    private void TransitionToNewPlaylist(RubanPlaylist playlist, bool clearQueue)
    {
        if (activePlaylistCount == 0)
        {
            StartNewPlaylist(playlist);
            return;
        }

        if (clearQueue)
        {
            queue.Clear();
            GetLastActive().onBoutDuRouleau = null;
            //GetLastActive().shouldLoop = false;
        }

        CheckIfActive(playlist);
        CheckIfTransitioning();

        activePlaylists[0].End(delegate (float height)
        {
            //Add new playlist
            activePlaylists[1] = playlist;
            activePlaylistCount++;

            //activePlaylists[0].deltaHeightAnchor = -heightAnchor;

            //heightAnchor = 0;
            activePlaylists[1].deltaHeightAnchor = -heightAnchor;
            activePlaylists[1].StartAt(height);
            activePlaylists[1].shouldLoop = true;
        },
        delegate ()
        {
            //Remove old playlist
            activePlaylists[0] = activePlaylists[1];
            activePlaylists[1] = null;
            activePlaylistCount--;
        });
    }
    #endregion

    #region Mode Queue

    public void QueueNewPlaylist(string name)
    {
        QueueNewPlaylist(GetPlaylistByName(name));
    }

    private void QueueNewPlaylist(RubanPlaylist playlist)
    {
        if (activePlaylistCount == 0)
        {
            StartNewPlaylist(playlist);
        }
        else
        {
            queue.Add(playlist);
        }
        GetLastActive().onBoutDuRouleau = OnQueueItemEnd;
    }

    private void OnQueueItemEnd(float height)
    {
        RubanPlaylist last = GetLastActive();

        last.onBoutDuRouleau = null;

        if (queue.Count <= 0)
        {
            last.shouldLoop = true;
            last.onBoutDuRouleau = OnQueueItemEnd;
        }
        else
        {
            if (queue[0] == last)
            {
                queue.RemoveAt(0);
                last.shouldLoop = true;
                last.onBoutDuRouleau = OnQueueItemEnd;
            }
            else
            {
                last.shouldLoop = false;
                queue[0].onBoutDuRouleau = OnQueueItemEnd;
                TransitionToNewPlaylist(queue[0], false);
                queue.RemoveAt(0);
            }
        }
    }

    #endregion

    #region Internal

    private bool IsActive(RubanPlaylist playlist)
    {
        for (int i = 0; i < activePlaylistCount; i++)
        {
            if (activePlaylists[i] == playlist)
                return true;
        }
        return false;
    }

    private void ResetHeightAnchor()
    {
        for (int i = 0; i < activePlaylistCount; i++)
        {
            activePlaylists[i].deltaHeightAnchor -= heightAnchor;
        }
        heightAnchor += ANCHORCAP;
        targetStopDistance += ANCHORCAP;
    }

    private RubanPlaylist GetLastActive()
    {
        return activePlaylists[activePlaylistCount - 1];
    }

    private RubanPlaylist GetPlaylistByName(string name)
    {
        int i = 0;
        for (i = 0; i < playlists.Count; i++)
        {
            if (playlists[i].name == name)
                break;
        }
        if (i >= playlists.Count)
            throw new System.Exception("Could not find a ruban playlist of name: " + name);

        return playlists[i];
    }

    private void CheckIfActive(RubanPlaylist playlist)
    {
        if (IsActive(playlist))
            throw new System.Exception("The following playlist is already active: " + playlist.name);
    }

    private void CheckIfTransitioning()
    {
        if (IsTransitionning())
            throw new System.Exception("Cannot transition to new rubanPlaylist. Already in transition.");
    }

    #endregion
}
