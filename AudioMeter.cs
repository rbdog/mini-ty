using System;
using System.Collections.Generic;
using UnityEngine;

// README.md
/*------------------------------------------------------
# How to Use

## AudioMeter.cs を好きな場所に置く

## 再生したい音楽ファイル xxx.wav を Assets/Resources/ 配下に置く 
- xxx.wav ファイルは Unity に取り込んだ場合に AudioClip xxx になる
- 配置例: Assets/Resources/aaa/bbb/xxx

## AudioMeter インスタンス化
// 新しいメーターを作る
// addTo: コンポーネントのアタッチ先 適当なオブジェクト
// resourcePath: AudioCip の Assets/Resources/ からの相対パス
// valueCount : 測定したい周波数レベルの数 1-20 程度
```{}
var meter = AudioMeter.init (
            addTo: this.gameObject,
            resourcePath: "aaa/bbb/xxx", 
            valueCount : 10);
```

## メソッド実行 再生
// 音楽再生&測定開始
```{}
meter.play (onUpdate: (values) =>
{
    // ここに更新処理をかく 
    // values: 周波数レベルごとの値配列 [2, 5, 3, 4 ... 1]
});
```

## メソッド実行 停止
// 音楽停止&測定停止
```{}
meter.stop();
```
------------------------------------------------------*/

/// AudioMeter
public class AudioMeter : MonoBehaviour
{
    // コンストラクタで指定させないデフォルト値
    int resolution = 256; // 大きくすると負荷と引き換えにクオリティが上がる 2のべき乗を設定
    float maxHz = 44100; // 対象の最大周波数
    float maxEnhance = 100; // 高音域の値を強調するための係数
    float _updateInterval = 0.05f; // 処理の頻度 秒数

    // 初期化時に決定する値 キャッシュ
    float deltaHz;
    float deltaEnhance;
    float deltaFreq;
    float[] freqs;
    private int _valueCount;
    private Action<float[]> _onUpdate;
    FreqRange[] ranges;
    AudioSource audioSource;

    // 処理の頻度を測るためのプロパティ
    float totalTime = 0f;
    
    class FreqRange
    {
        public float from;
        public float to;
        public FreqRange (float from, float to)
        {
            this.from = from;
            this.to = to;
        }
    }

    /// 引数付き初期化
    public static AudioMeter init (GameObject addTo, string resourcePath, int valueCount)
    {
        var meter = addTo.AddComponent<AudioMeter> ();
        meter.audioSource = meter.gameObject.AddComponent<AudioSource> ();
        meter._valueCount = valueCount;
        meter.deltaHz = meter.maxHz / ((float) valueCount);
        meter.deltaEnhance = meter.maxEnhance / ((float) (valueCount - 1));
        meter.deltaFreq = AudioSettings.outputSampleRate / meter.resolution;

        // clip
        var clip = Resources.Load<AudioClip> (resourcePath);
        if (clip == null)
        {
            Debug.Log ("could not find AudioClip at Resources/_resourcePath_ ");
        }
        meter.audioSource.clip = clip;
        // freqs
        var freqs = new float[meter.resolution];
        for (var i = 0; i < meter.resolution; i++)
        {
            var freq = meter.deltaFreq * (i + 1);
            freqs[i] = freq;
        }
        meter.freqs = freqs;
        // ranges
        var ranges = new FreqRange[meter._valueCount];
        for (var i = 0; i < meter._valueCount; i++)
        {
            var from = meter.deltaHz * i;
            var to = meter.deltaHz * (i + 1);
            ranges[i] = new FreqRange (from: from, to: to);
        }
        meter.ranges = ranges;

        return meter;
    }
    
    /// 音楽再生 & 再生測定開始
    public void play (Action<float[]> onUpdate)
    {
        this._onUpdate = onUpdate;
        this.audioSource.Play ();
    }

    /// 音楽停止 & 測定停止
    public void stop ()
    {
        this.audioSource.Stop ();
    }

    /// 周波数ごとの数値配列
    float[] values (float[] of )
    {
        var values = new float[this._valueCount];
        var f = 0;
        foreach (var freq in this.freqs)
        {
            var r = 0;
            foreach (var range in this.ranges)
            {
                if (range.from < freq && freq < range.to)
                {
                    values[r] = values[r] + of [f];
                    continue;
                }
                r++;
            }
            f++;
        }
        return values;
    }

    /// 高音域が強調された数値配列
    float[] enhancedValues (float[] of )
    {
        var eValues = new float[this._valueCount];
        for (var i = 0; i < this._valueCount; i++)
        {
            eValues[i] = of [i] * this.deltaEnhance * (i + 1);
        }
        return eValues;
    }

    void Start ()
    { }

    void Update ()
    {
        this.totalTime += Time.deltaTime;
        if ((this._updateInterval < totalTime) && (this.audioSource.isPlaying))
        {
            var spectrum = new float[this.resolution];
            this.audioSource.GetSpectrumData (spectrum, 0, FFTWindow.Rectangular);
            var values = this.values ( of: spectrum);
            var enhancedValues = this.enhancedValues ( of: values);
            this._onUpdate(enhancedValues);

            totalTime = 0f;
        }
    }
}