namespace UNITy {
	////using System.Dynamic;
	using System;
	using System.Reflection;
	using UnityEngine;
	using UnityObject = UnityEngine.Object;

	public class Accessor<T> where T : Component, new() {
		private static readonly T INSTANCE;

		static Accessor() {
			INSTANCE = (T)UnityObject.FindObjectOfType(typeof (T));
		}

		public void GetMember<U>(string name, out U result) {
			result = default(U);
			try {
				FieldInfo field = typeof (T).GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
				result = (U)field.GetValue(INSTANCE);
			} catch (Exception e) {
				throw new Exception(string.Format("Error getting member: {0}", name), e);
			}
		}

		public void SetMember<U>(string name, U value) {
			try {
				FieldInfo field = typeof (T).GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
				field.SetValue(INSTANCE, value);
			} catch (Exception e) {
				throw new Exception(string.Format("Error setting member: {0}", name), e);
			}
		}

		public U InvokeMember<U>(string name, ref object[] args) {
			try {
				MethodInfo method = typeof (T).GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
				if (method.ReturnType == typeof (void)) {
					method.Invoke(INSTANCE, args);
				} else {
					return (U)method.Invoke(INSTANCE, args);
				}
			} catch (Exception e) {
				throw new Exception(string.Format("Error invoking member: {0}", name), e);
			}

			return default(U);
		}

		public void InvokeMember(string name) {
			try {
				MethodInfo method = typeof (T).GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
				method.Invoke(INSTANCE, new object[] { });
			} catch (Exception e) {
				throw new Exception(string.Format("Error invoking member: {0}", name), e);
			}
		}

		// TODO: Use when .Net 4.0 is Supported in Unity
		////public override bool TryGetMember(GetMemberBinder binder, out object result) {
		////    result = null;
		////    try {
		////        FieldInfo field = typeof (T).GetField(binder.Name);
		////        result = field.GetValue(INSTANCE);
		////    } catch {
		////        return false;
		////    }

		////    return true;
		////}

		////public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result) {
		////    result = null;

		////    try {
		////        MethodInfo method = typeof (T).GetMethod(binder.Name, BindingFlags.Public | BindingFlags.NonPublic);
		////        result = method.Invoke(INSTANCE, args);
		////    } catch {
		////        return false;
		////    }

		////    return true;
		////}
	}
}