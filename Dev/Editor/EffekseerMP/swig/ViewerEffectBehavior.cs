//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.6
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace Effekseer.swig {

public class ViewerEffectBehavior : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal ViewerEffectBehavior(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(ViewerEffectBehavior obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~ViewerEffectBehavior() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          EffekseerNativePINVOKE.delete_ViewerEffectBehavior(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public int CountX {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_CountX_set(swigCPtr, value);
    } 
    get {
      int ret = EffekseerNativePINVOKE.ViewerEffectBehavior_CountX_get(swigCPtr);
      return ret;
    } 
  }

  public int CountY {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_CountY_set(swigCPtr, value);
    } 
    get {
      int ret = EffekseerNativePINVOKE.ViewerEffectBehavior_CountY_get(swigCPtr);
      return ret;
    } 
  }

  public int CountZ {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_CountZ_set(swigCPtr, value);
    } 
    get {
      int ret = EffekseerNativePINVOKE.ViewerEffectBehavior_CountZ_get(swigCPtr);
      return ret;
    } 
  }

  public int TimeSpan {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_TimeSpan_set(swigCPtr, value);
    } 
    get {
      int ret = EffekseerNativePINVOKE.ViewerEffectBehavior_TimeSpan_get(swigCPtr);
      return ret;
    } 
  }

  public byte AllColorR {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_AllColorR_set(swigCPtr, value);
    } 
    get {
      byte ret = EffekseerNativePINVOKE.ViewerEffectBehavior_AllColorR_get(swigCPtr);
      return ret;
    } 
  }

  public byte AllColorG {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_AllColorG_set(swigCPtr, value);
    } 
    get {
      byte ret = EffekseerNativePINVOKE.ViewerEffectBehavior_AllColorG_get(swigCPtr);
      return ret;
    } 
  }

  public byte AllColorB {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_AllColorB_set(swigCPtr, value);
    } 
    get {
      byte ret = EffekseerNativePINVOKE.ViewerEffectBehavior_AllColorB_get(swigCPtr);
      return ret;
    } 
  }

  public byte AllColorA {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_AllColorA_set(swigCPtr, value);
    } 
    get {
      byte ret = EffekseerNativePINVOKE.ViewerEffectBehavior_AllColorA_get(swigCPtr);
      return ret;
    } 
  }

  public float Distance {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_Distance_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_Distance_get(swigCPtr);
      return ret;
    } 
  }

  public int RemovedTime {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_RemovedTime_set(swigCPtr, value);
    } 
    get {
      int ret = EffekseerNativePINVOKE.ViewerEffectBehavior_RemovedTime_get(swigCPtr);
      return ret;
    } 
  }

  public float PositionX {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_PositionX_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_PositionX_get(swigCPtr);
      return ret;
    } 
  }

  public float PositionY {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_PositionY_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_PositionY_get(swigCPtr);
      return ret;
    } 
  }

  public float PositionZ {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_PositionZ_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_PositionZ_get(swigCPtr);
      return ret;
    } 
  }

  public float RotationX {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_RotationX_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_RotationX_get(swigCPtr);
      return ret;
    } 
  }

  public float RotationY {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_RotationY_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_RotationY_get(swigCPtr);
      return ret;
    } 
  }

  public float RotationZ {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_RotationZ_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_RotationZ_get(swigCPtr);
      return ret;
    } 
  }

  public float ScaleX {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_ScaleX_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_ScaleX_get(swigCPtr);
      return ret;
    } 
  }

  public float ScaleY {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_ScaleY_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_ScaleY_get(swigCPtr);
      return ret;
    } 
  }

  public float ScaleZ {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_ScaleZ_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_ScaleZ_get(swigCPtr);
      return ret;
    } 
  }

  public float PositionVelocityX {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_PositionVelocityX_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_PositionVelocityX_get(swigCPtr);
      return ret;
    } 
  }

  public float PositionVelocityY {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_PositionVelocityY_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_PositionVelocityY_get(swigCPtr);
      return ret;
    } 
  }

  public float PositionVelocityZ {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_PositionVelocityZ_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_PositionVelocityZ_get(swigCPtr);
      return ret;
    } 
  }

  public float RotationVelocityX {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_RotationVelocityX_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_RotationVelocityX_get(swigCPtr);
      return ret;
    } 
  }

  public float RotationVelocityY {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_RotationVelocityY_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_RotationVelocityY_get(swigCPtr);
      return ret;
    } 
  }

  public float RotationVelocityZ {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_RotationVelocityZ_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_RotationVelocityZ_get(swigCPtr);
      return ret;
    } 
  }

  public float ScaleVelocityX {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_ScaleVelocityX_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_ScaleVelocityX_get(swigCPtr);
      return ret;
    } 
  }

  public float ScaleVelocityY {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_ScaleVelocityY_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_ScaleVelocityY_get(swigCPtr);
      return ret;
    } 
  }

  public float ScaleVelocityZ {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_ScaleVelocityZ_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_ScaleVelocityZ_get(swigCPtr);
      return ret;
    } 
  }

  public float TargetPositionX {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_TargetPositionX_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_TargetPositionX_get(swigCPtr);
      return ret;
    } 
  }

  public float TargetPositionY {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_TargetPositionY_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_TargetPositionY_get(swigCPtr);
      return ret;
    } 
  }

  public float TargetPositionZ {
    set {
      EffekseerNativePINVOKE.ViewerEffectBehavior_TargetPositionZ_set(swigCPtr, value);
    } 
    get {
      float ret = EffekseerNativePINVOKE.ViewerEffectBehavior_TargetPositionZ_get(swigCPtr);
      return ret;
    } 
  }

  public ViewerEffectBehavior() : this(EffekseerNativePINVOKE.new_ViewerEffectBehavior(), true) {
  }

}

}
