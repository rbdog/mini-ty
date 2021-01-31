// 
// Player.cs
// 
/****************************************

- Enable the "InputSystem"
    InputSystem を 有効 にする

- Import MCUnitychan
    MC Unity Chan を インポート する
    https://unity-chan.com/download/releaseNote.php?id=MCUnitychan

- Attach this script to MCUnityChan model
    このスクリプトを MC Unity Chan のモデルに貼り付ける

*****************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static UnityEngine.InputSystem.Keyboard;

//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////
// Player
//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////
public partial class Player : IPlayer
{
  // 入力に使うキー
  virtual public Input[] keyInputs ()
  {
    return new Input[]
    {
      new KeyInput.T (), // T - テスト用
        new KeyInput.W (), // W
        new KeyInput.A (), // A
        new KeyInput.S (), // S
        new KeyInput.D (), // D
        new KeyInput.Space (), // スペースキー
        new KeyInput.I (), // I
        new KeyInput.J (), // J
        new KeyInput.K (), // K
        new KeyInput.L (), // L
        new KeyInput.U (), // U
    };
  }
}

//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////
// IPlayer.cs
//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////
// プレイヤーに対する操作一覧
public interface IPlayer
{
  // 足が地面についているかどうかを取得
  public bool isGrounded ();
  // どちらを見ているかを取得
  public To direction ();
  // to方向 に deltaTime秒間 移動する
  public void move (To to, float deltaTime);
  // ジャンプ
  public void jump ();
  // to方向 に 体を向ける
  public void turn (To to);
  // 上方を見る deltaTime秒間
  public void lookU (float deltaTime);
  // 左方を見る deltaTime秒間
  public void lookL (float deltaTime);
  // 下方を見る deltaTime秒間
  public void lookD (float deltaTime);
  // 右方を見る deltaTime秒間
  public void lookR (float deltaTime);
  // id の アニメーションを開始する
  public void startAnimation (AnimationID id);
  // id の アニメーションを終了する
  public void stopAnimation (AnimationID id);
  // to方向 に 歩くアニメーションをしているか
  public bool isWalking (To to);
}

// プレイヤーの向き(カメラ基準)
public enum To
{
  // 前
  F,
  // 左
  L,
  // 右
  R,
  // 後
  B
}

// プレイヤーのアニメーションID
public enum AnimationID
{
  // 歩く
  walk,
  // 手を前に出す
  touch

  // ToDo: - 気が向いたら追加します
}

/////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////
// キーボード入力
/////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////
namespace KeyInput
{
  // T
  public class T : Input
  {
    // 1
    public KeyControl key ()
    {
      return current.tKey; // キーボードの「T」を待ち受ける : tKey
    }

    // 2
    public void onStart (IPlayer player)
    {
      Debug.Log (" T を押し始めました");
    }

    // 3
    public void onMiddle (IPlayer player, float deltaTime)
    {
      Debug.Log (" T を押しています");
    }

    // 4
    public void onEnd (IPlayer player)
    {
      Debug.Log (" T を押し終わりました");
    }
  }

  // W
  public class W : Input
  {
    public KeyControl key ()
    {
      return current.wKey;
    }
    public void onStart (IPlayer player)
    {
      player.stopAnimation (AnimationID.walk);
      player.turn (To.F);
      player.startAnimation (AnimationID.walk);
    }
    public void onMiddle (IPlayer player, float deltaTime)
    {
      player.move (To.F, deltaTime);
    }
    public void onEnd (IPlayer player)
    {
      if (!player.isWalking (To.L) && !player.isWalking (To.B) && !player.isWalking (To.R))
      {
        player.stopAnimation (AnimationID.walk);
      }
    }
  }

  // A
  public class A : Input
  {
    public KeyControl key ()
    {
      return current.aKey;
    }
    public void onStart (IPlayer player)
    {
      player.stopAnimation (AnimationID.walk);
      player.turn (To.L);
      player.startAnimation (AnimationID.walk);
    }
    public void onMiddle (IPlayer player, float deltaTime)
    {
      player.move (To.L, deltaTime);
    }
    public void onEnd (IPlayer player)
    {
      if (!player.isWalking (To.F) && !player.isWalking (To.B) && !player.isWalking (To.R))
      {
        player.stopAnimation (AnimationID.walk);
      }
    }
  }

  // S
  public class S : Input
  {
    public KeyControl key ()
    {
      return current.sKey;
    }
    public void onStart (IPlayer player)
    {
      player.stopAnimation (AnimationID.walk);
      player.turn (To.B);
      player.startAnimation (AnimationID.walk);
    }
    public void onMiddle (IPlayer player, float deltaTime)
    {
      player.move (To.B, deltaTime);
    }
    public void onEnd (IPlayer player)
    {
      if (!player.isWalking (To.F) && !player.isWalking (To.L) && !player.isWalking (To.R))
      {
        player.stopAnimation (AnimationID.walk);
      }
    }
  }

  // D
  public class D : Input
  {
    public KeyControl key ()
    {
      return current.dKey;
    }
    public void onStart (IPlayer player)
    {
      player.stopAnimation (AnimationID.walk);
      player.turn (To.R);
      player.startAnimation (AnimationID.walk);
    }
    public void onMiddle (IPlayer player, float deltaTime)
    {
      player.move (To.R, deltaTime);
    }
    public void onEnd (IPlayer player)
    {
      if (!player.isWalking (To.F) && !player.isWalking (To.L) && !player.isWalking (To.B))
      {
        player.stopAnimation (AnimationID.walk);
      }
    }
  }

  // Space
  public class Space : Input
  {
    public KeyControl key ()
    {
      return current.spaceKey;
    }
    virtual public void onStart (IPlayer player)
    {
      player.jump ();
    }
    virtual public void onMiddle (IPlayer player, float deltaTime)
    { }
    virtual public void onEnd (IPlayer player)
    { }
  }

  // I
  public class I : Input
  {
    public KeyControl key ()
    {
      return current.iKey;
    }
    virtual public void onStart (IPlayer player)
    { }
    virtual public void onMiddle (IPlayer player, float deltaTime)
    {
      player.lookU (deltaTime);
    }
    virtual public void onEnd (IPlayer player)
    { }
  }

  // K
  public class K : Input
  {
    public KeyControl key ()
    {
      return current.kKey;
    }
    virtual public void onStart (IPlayer player)
    { }
    virtual public void onMiddle (IPlayer player, float deltaTime)
    {
      player.lookD (deltaTime);
    }
    virtual public void onEnd (IPlayer player)
    { }
  }

  // L
  public class L : Input
  {
    public KeyControl key ()
    {
      return current.lKey;
    }
    virtual public void onStart (IPlayer player)
    { }
    virtual public void onMiddle (IPlayer player, float deltaTime)
    {
      player.lookR (deltaTime);
    }
    virtual public void onEnd (IPlayer player)
    { }
  }

  // J
  public class J : Input
  {
    public KeyControl key ()
    {
      return current.jKey;
    }
    virtual public void onStart (IPlayer player)
    { }
    virtual public void onMiddle (IPlayer player, float deltaTime)
    {
      player.lookL (deltaTime);
    }
    virtual public void onEnd (IPlayer player)
    { }
  }

  // U
  public class U : Input
  {
    public KeyControl key ()
    {
      return current.uKey;
    }
    virtual public void onStart (IPlayer player)
    {
      player.startAnimation (AnimationID.touch);
    }
    virtual public void onMiddle (IPlayer player, float deltaTime)
    { }
    virtual public void onEnd (IPlayer player)
    {
      player.stopAnimation (AnimationID.touch);
    }
  }
}

/////////////////////////////////////////////////////////////////////
// Bone
/////////////////////////////////////////////////////////////////////
namespace Bone
{
  // IBone
  interface IBone
  {
    void angle (float to);
    void resetRotation ();
  }
  // Base
  public abstract class Base : IBone
  {
    public IPlayer mcPlayer;
    public HumanBodyBones rawBone;
    public Quaternion initialRotation;
    public To initialTo;
    public Transform transform;
    public Transform fAnchor;

    public Base (IPlayer mcPlayer, HumanBodyBones b)
    {
      this.mcPlayer = mcPlayer;
      this.rawBone = b;
      var animator = ((Player) this.mcPlayer).gameObject.GetComponent<Animator> ();
      this.transform = animator.GetBoneTransform (b);
      this.fAnchor = ((Player) this.mcPlayer).fAnchorTransform;
    }
    public void resetRotation ()
    {
      var direction = ((Player) this.mcPlayer).direction ();
      this.mcPlayer.turn (this.initialTo);
      this.transform.rotation = this.initialRotation;
      this.mcPlayer.turn (direction);
    }
    public void angle (float to)
    {
      var now = this.transform.rotation.eulerAngles.x;
      var diff = to - now;
      this.transform.Rotate (this.fAnchor.up, -diff);
    }
  }

  // Arm
  namespace Arm
  {
    // L
    public class L : Base
    {
      public L (IPlayer player) : base (player, HumanBodyBones.LeftUpperArm)
      {
        this.initialRotation = this.transform.rotation;
      }
    }
    // R
    public class R : Base
    {
      public R (IPlayer player) : base (player, HumanBodyBones.RightUpperArm)
      {
        this.initialRotation = this.transform.rotation;
      }
    }
  }

  // Leg
  namespace Leg
  {
    // L
    public class L : Base
    {
      public L (IPlayer player) : base (player, HumanBodyBones.LeftUpperLeg)
      {
        this.transform.Rotate (this.fAnchor.right, 90);
        this.transform.Rotate (this.fAnchor.forward, 6);
        this.transform.Rotate (-this.fAnchor.up, 7);
        this.initialRotation = this.transform.rotation;
      }
    }
    // R
    public class R : Base
    {
      public R (IPlayer player) : base (player, HumanBodyBones.RightUpperLeg)
      {
        this.transform.Rotate (-this.fAnchor.right, 90);
        this.transform.Rotate (this.fAnchor.forward, 6);
        this.transform.Rotate (this.fAnchor.up, 7);
        this.initialRotation = this.transform.rotation;
      }
    }
  }

  // Bones
  public class Bones
  {
    public Arm.L armL;
    public Arm.R armR;
    public Leg.L legL;
    public Leg.R legR;

    public Bones (IPlayer player)
    {
      armL = new Arm.L (player);
      armR = new Arm.R (player);
      legL = new Leg.L (player);
      legR = new Leg.R (player);
    }
  }
}

// Animation
namespace Animation
{
  // IAnimation
  interface IAnimation
  {
    int priority ();
    AnimationID id ();
    void play (float deltaTime);
    void stop ();
  }
  // Base
  public class Base
  {
    public Bone.Bones bones;
    public Base (Bone.Bones bones)
    {
      this.bones = bones;
    }
  }

  // Walk
  public partial class Walk : Base
  {
    VibrationValue walkAngle = new VibrationValue (-45, 45, 0);
    float speed = 200;
    public Walk (Bone.Bones bones) : base (bones)
    { }
    void setWalkAngles (float to)
    {
      this.bones.armR.angle (to);
      this.bones.armL.angle (to);
      this.bones.legL.angle (to);
      this.bones.legR.angle (to);
    }
  }
  // Walk + IAnimation
  public partial class Walk : IAnimation
  {
    public int priority ()
    {
      return 100;
    }
    public AnimationID id ()
    {
      return AnimationID.walk;
    }
    public void play (float deltaTime)
    {
      this.walkAngle.next (deltaTime * this.speed);
      this.setWalkAngles (walkAngle.value);
    }
    public void stop ()
    {
      this.bones.armR.resetRotation ();
      this.bones.armL.resetRotation ();
      this.bones.legL.resetRotation ();
      this.bones.legR.resetRotation ();
    }
  }

  // Touch
  public partial class Touch : Base
  {
    VibrationValue angle = new VibrationValue (0, 85, 1);
    float speed = 300;
    public Touch (Bone.Bones bones) : base (bones)
    { }
  }
  // Touch + IAnimation
  public partial class Touch : IAnimation
  {
    public int priority ()
    {
      return 200;
    }
    public AnimationID id ()
    {
      return AnimationID.touch;
    }
    public void play (float deltaTime)
    {
      this.angle.next (deltaTime * this.speed);
      this.bones.armR.angle (angle.value);
    }
    public void stop ()
    {
      this.bones.armR.resetRotation ();
    }
  }

  // IUpdater
  public interface IUpdater
  {
    void update (float deltaTime);
    void start (AnimationID id);
    void stop (AnimationID id);
  }

  // Updater
  public class Updater : IUpdater
  {
    public Bone.Bones bones;
    List<IAnimation> anims = new List<IAnimation> ();

    public Updater (IPlayer player)
    {
      this.bones = new Bone.Bones (player);
    }

    // 優先度が一番高いアニメーションのみ再生する
    public void update (float deltaTime)
    {
      if (0 < this.anims.Count)
      {
        var maxPriorityAnim = this.anims.maxFilter (a => a.priority ());
        maxPriorityAnim.play (deltaTime);
      }
    }
    public void start (AnimationID id)
    {
      switch (id)
      {
        case AnimationID.walk:
          this.anims.Add (new Animation.Walk (this.bones));
          break;
        case AnimationID.touch:
          this.anims.Add (new Animation.Touch (this.bones));
          break;
      }
    }
    public void stop (AnimationID id)
    {
      if (0 < this.anims.Count)
      {
        var anims = this.anims.FindAll (a => a.id ().Equals (id));
        foreach (IAnimation anim in anims)
        {
          anim.stop ();
        }
        this.anims.RemoveAll (a => a.id ().Equals (id));
      }
    }
  }
}

// Input
public interface Input
{
  // key for input
  public KeyControl key ();
  // start of pushing
  public void onStart (IPlayer player);
  // pushing
  public void onMiddle (IPlayer player, float deltaTime);
  // end of pushing
  public void onEnd (IPlayer player);
}

//***************************************************************
// Player.cs
//***************************************************************
// abstract Player
public partial class Player : MonoBehaviour
{

  // CACHE
  CharacterController controller;
  Input[] inputs;
  float ySpeed = 0;
  Camera mainCamera;
  public Transform fAnchorTransform;
  Animation.IUpdater animUpdater;
  public Bone.Bones bones;
  public HashSet<To> walkingHashes = new HashSet<To> ();

  // SETTING
  public float moveSpeed ()
  {
    return 5.0f;
  }
  public float jumpHeight ()
  {
    return 1.0f;
  }
  public float gravity ()
  {
    return 9.8f;
  }
  public float lookSpeed ()
  {
    return 50.0f;
  }
  public float height ()
  {
    return 2.0f;
  }

  // anchor object for definition of forward derection
  public GameObject forwardAnchor ()
  {
    return Camera.main.gameObject;
  }

  /// MonoBehaviour Start()
  void Start ()
  {
    this.controller = this.gameObject.AddComponent<CharacterController> ();
    this.controller.center = new Vector3 (0, (this.height () / 2), 0);
    this.controller.height = this.height ();
    this.controller.skinWidth = 0.01f;
    this.controller.radius = 0.5f;
    this.inputs = this.keyInputs ();
    this.fAnchorTransform = this.forwardAnchor ().transform;
    this.mainCamera = Camera.main;
    this.animUpdater = new Animation.Updater (this);
    this.bones = new Bone.Bones (this);
  }

  /// MonoBehaviour Update()
  void Update ()
  {

    // refresh ySpeed
    if (this.isGrounded () && this.ySpeed < 0)
    {
      this.ySpeed = 0;
    }
    else
    {
      this.ySpeed -= this.gravity () * Time.deltaTime;
    }

    // use gravity
    var v3 = new Vector3 (0, this.ySpeed, 0);
    this.controller.Move (v3 * Time.deltaTime);

    // check input
    foreach (var input in this.inputs)
    {
      if (input.key ().wasPressedThisFrame)
      {
        input.onStart (this);
      }
      if (input.key ().isPressed)
      {
        input.onMiddle (this, Time.deltaTime);
      }
      if (input.key ().wasReleasedThisFrame)
      {
        input.onEnd (this);
      }
    }

    // animation
    if (this.animUpdater != null)
    {
      this.animUpdater.update (Time.deltaTime);
    }
  }
}

//***************************************************************
// Player+IPlayer.cs
//***************************************************************
public partial class Player : IPlayer
{
  public bool isGrounded ()
  {
    return this.controller.isGrounded;
  }
  public To direction ()
  {
    var form = this.transform;
    var diffB = (-(this.fAnchorTransform.forward) - form.forward).sqrMagnitude;
    var diffR = (this.fAnchorTransform.right - form.forward).sqrMagnitude;
    var diffF = (this.fAnchorTransform.forward - form.forward).sqrMagnitude;
    var diffL = (-(this.fAnchorTransform.right) - form.forward).sqrMagnitude;
    var minDiff = diffF;
    var minTo = To.F;
    if (diffL < minDiff)
    {
      minDiff = diffL;
      minTo = To.L;
    }
    if (diffB < minDiff)
    {
      minDiff = diffB;
      minTo = To.B;
    }
    if (diffR < minDiff)
    {
      minDiff = diffR;
      minTo = To.R;
    }
    return minTo;
  }
  public void move (To to, float dTime)
  {
    var direction = Vector3.zero;
    switch (to)
    {
      case To.F:
        direction = this.fAnchorTransform.forward;
        break;
      case To.L:
        direction = -(this.fAnchorTransform.right);
        break;
      case To.B:
        direction = -(this.fAnchorTransform.forward);
        break;
      case To.R:
        direction = this.fAnchorTransform.right;
        break;
    }
    var v3 = direction * this.moveSpeed ();
    this.controller.Move (v3 * dTime);
  }
  public void jump ()
  {
    if (this.isGrounded ())
    {
      this.ySpeed += Mathf.Sqrt (this.jumpHeight () * 3.0f * this.gravity ());
    }
  }
  public void turn (To to)
  {
    var direction = Vector3.zero;
    switch (to)
    {
      case To.F:
        direction = this.fAnchorTransform.forward;
        break;
      case To.L:
        direction = -(this.fAnchorTransform.right);
        break;
      case To.B:
        direction = -(this.fAnchorTransform.forward);
        break;
      case To.R:
        direction = this.fAnchorTransform.right;
        break;
    }
    var v3 = new Vector3 (direction.x, 0, direction.z);

    this.mainCamera.transform.parent = null;
    this.transform.forward = v3;
    this.mainCamera.transform.parent = this.transform;
  }
  public void lookL (float deltaTime)
  {
    // 右を向く速さは2倍の速度にした(* 2.0)
    this.mainCamera.transform.RotateAround (this.transform.position, -(Vector3.up), this.lookSpeed () * deltaTime * 2.0f);
  }
  public void lookR (float deltaTime)
  {
    // 右を向く速さは2倍の速度にした(* 2.0)
    this.mainCamera.transform.RotateAround (this.transform.position, Vector3.up, this.lookSpeed () * deltaTime * 2.0f);
  }
  public void lookU (float deltaTime)
  {
    this.mainCamera.transform.RotateAround (this.transform.position, -(this.fAnchorTransform.right), this.lookSpeed () * deltaTime);
  }
  public void lookD (float deltaTime)
  {
    this.mainCamera.transform.RotateAround (this.transform.position, this.fAnchorTransform.right, this.lookSpeed () * deltaTime);
  }

  public void startAnimation (AnimationID id)
  {
    if (id == AnimationID.walk)
    {
      this.walkingHashes.Add (this.direction ());
    }
    this.animUpdater.start (id);
  }
  public void stopAnimation (AnimationID id)
  {
    this.animUpdater.stop (id);
    if (id == AnimationID.walk)
    {
      this.walkingHashes.Remove (this.direction ());
    }
  }
  public bool isWalking (To to)
  {
    var isWaking = this.walkingHashes.Contains (to);
    return isWaking;
  }
}

/////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////
// その他汎用機能
/////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////

/// 最大値や最小値をプロパティにもつ要素を返す拡張
public static class IEnumerableExtensions
{
  public static TSource minFilter<TSource, TResult> (
    this IEnumerable<TSource> self,
    Func<TSource, TResult> selector
  )
  {
    return self.First (c => selector (c).Equals (self.Min (selector)));
  }

  public static TSource maxFilter<TSource, TResult> (
    this IEnumerable<TSource> self,
    Func<TSource, TResult> selector
  )
  {
    return self.First (c => selector (c).Equals (self.Max (selector)));
  }
}

/// 振動する値を表す float の ValueObject
class VibrationValue
{
  float min = -90.0f;
  float max = 90.0f;
  public float value = 0.0f;
  bool isIncreasing = true;

  public VibrationValue (float min, float max, float startValue)
  {
    this.min = min;
    this.max = max;
    this.value = startValue;
  }
  public void next (float value)
  {
    if (this.isIncreasing)
    {
      this.value += value;
      if (this.max < this.value)
      {
        this.isIncreasing = false;
        var diff = this.value - this.max;
        this.value -= diff;
      }
    }
    else
    {
      this.value -= value;
      if (this.value < this.min)
      {
        this.isIncreasing = true;
        var diff = this.min - this.value;
        this.value += diff;
      }
    }
  }
}