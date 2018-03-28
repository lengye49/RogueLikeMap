using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class CharacterAnimation : MonoBehaviour
{
    private Image ImageSource;
    private int mCurFrame = 0;
    private float mDelta = 0;

    private int startFrame;
    private int endFrame;

    public float FPS = 5;
    public List<Sprite> SpriteFrames;

    private bool IsPlaying = true;
    private bool AutoPlay = true;
    private bool Loop = true;

    private bool isChanged = false;

    private float t;

    void Awake()
    {
        ImageSource = GetComponent<Image>();
    }

    void Start()
    {
        Rest();
        t = 0f;
    }

    void Rest(){
        isChanged = true;
        startFrame = 0;
        endFrame = 3;
    }

    void Run(){
        isChanged = true;
        startFrame = 4;
        endFrame = 11;
    }

    private void SetSprite(int idx)
    {
        ImageSource.sprite = SpriteFrames[idx];
        //该部分为设置成原始图片大小，如果只需要显示Image设定好的图片大小，注释掉该行即可。
        ImageSource.SetNativeSize();
    }



//    public void PlayReverse()
//    {
//        IsPlaying = true;
//        Foward = false;
//    }
    bool isRunning=false;
    void Update()
    {
        t += Time.deltaTime;
       
        if (t > 4.0f && !isRunning)
        {
            Run();
            isRunning = true;
        }


        if (!IsPlaying || startFrame == endFrame)
        {
            return;
        }

        mDelta += Time.deltaTime;
        if (mDelta > 1 / FPS)
        {
            mDelta = 0;

            mCurFrame++;

            if (mCurFrame > endFrame)
            {
//                if (Loop)
//                {
                    mCurFrame = startFrame;
//                }
//                else
//                {
//                    IsPlaying = false;
//                    return;
//                }
            }
//            else if (mCurFrame<0)
//            {
//                if (Loop)
//                {
//                    mCurFrame = FrameCount-1;
//                }
//                else
//                {
//                    IsPlaying = false;
//                    return;
//                }          
//            }
            if (isChanged)
            {
                mCurFrame = startFrame;
                isChanged = false;
            }

            SetSprite(mCurFrame);
        }
    }

//    public void Pause()
//    {
//        IsPlaying = false;
//    }
//
//    public void Resume()
//    {
//        if (!IsPlaying)
//        {
//            IsPlaying = true;
//        }
//    }
//
//    public void Stop()
//    {
//        mCurFrame = 0;
//        SetSprite(mCurFrame);
//        IsPlaying = false;
//    }
//
//    public void Rewind()
//    {
//        mCurFrame = 0;
//        SetSprite(mCurFrame);
//        Play();
//    }
}