using UnityEngine;
using System.Collections;

public class SoundManager {
    
    private static SoundManager inst = null;
    public AudioClip AC_Attack;
    public AudioClip AC_Music;

    public static SoundManager GetInst()
    {
        if (inst == null)
        {
            inst = new SoundManager();
            inst.Init();
        }

        return inst;
    }

    public void Init()
    {
        AC_Attack = (AudioClip)Resources.Load("Sound/Effect/Crash2");
        AC_Music = (AudioClip)Resources.Load("Sound/Music/Maple Story");
    }

    public void PlayAttackSound(Vector3 pos)
    {
        // 노말 클래스라 포지션이 없어 this.transform.position 사용 못함
        AudioSource.PlayClipAtPoint(AC_Attack, pos);
    }

    // 배경 음악
    public void PlayMusic(Vector3 pos)
    {
        //AudioSource.PlayClipAtPoint(AC_Music, pos);
    }
}
