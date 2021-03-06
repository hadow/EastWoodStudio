using System;
using System.Collections.Generic;
using Eluant;
namespace EW.Scripting
{

    /// <summary>
    /// �ӿ���չ��only for LuaValue)
    /// </summary>
    public static class LuaValueExts
    {
        public static Type WrappedClrType(this LuaValue value)
        {
            object inner;
            if (value.TryGetClrObject(out inner))
                return inner.GetType();

            return value.GetType();
        }


        /// <summary>
        /// CLR -> Lua
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static LuaValue ToLuaValue(this object obj,ScriptContext context)
        {
            if (obj is LuaValue)
                return (LuaValue)obj;

            if (obj == null)
                return LuaNil.Instance;

            if (obj is double)
                return (double)obj;

            if (obj is int)
                return (int)obj;

            if (obj is bool)
                return (bool)obj;

            if (obj is string)
                return (string)obj;

            if(obj is IScriptBindable)
            {
                var notify = obj as IScriptNotifyBind;
                if (notify != null)
                    notify.OnScriptBind(context);

                return new LuaCustomClrObject(obj);
            }

            if(obj is Array)
            {
                var array = (Array)obj;
                var i = 1;
                var table = context.CreateTable();

                foreach (var x in array)
                    using (LuaValue key = i++, value = x.ToLuaValue(context))
                        table.Add(key, value);
                return table;
            }
            throw new InvalidOperationException("Cannot convert type {0} to Lua,Class Must implement IScriptBindable.".F(obj.GetType()));
        }

        public static bool TryGetClrValue<T>(this LuaValue value,out T clrObject)
        {
            object temp;
            var ret = value.TryGetClrValue(typeof(T), out temp);
            clrObject = ret ? (T)temp : default(T);
            return ret;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="t"></param>
        /// <param name="clrObject"></param>
        /// <returns></returns>
        public static bool TryGetClrValue(this LuaValue value,Type t,out object clrObject)
        {
            object temp;

            var nullable = Nullable.GetUnderlyingType(t);
            if (nullable != null)
                t = nullable;

            //value wrapps a clr object
            if(value.TryGetClrObject(out temp))
            {
                if(temp.GetType() == t)
                {
                    clrObject = temp;
                    return true;
                }
            }

            if(value is LuaNil && !t.IsValueType)
            {
                clrObject = null;
                return true;
            }

            if(value is LuaBoolean && t.IsAssignableFrom(typeof(bool)))
            {
                clrObject = value.ToBoolean();
                return true;
            }

            if(value is LuaNumber && t.IsAssignableFrom(typeof(double)))
            {
                clrObject = value.ToNumber().Value;
                return true;
            }
            if(value is LuaNumber && t.IsAssignableFrom(typeof(int)))
            {
                clrObject = (int)value.ToNumber().Value;
                return true;
            }
            if(value is LuaString && t.IsAssignableFrom(typeof(string)))
            {
                clrObject = value.ToString();
                return true;
            }
            if(value is LuaFunction && t.IsAssignableFrom(typeof(LuaFunction)))
            {
                clrObject = value;
                return true;
            }
            if(value is LuaTable && t.IsAssignableFrom(typeof(LuaTable)))
            {
                clrObject = value;
                return true;
            }

            if(value is LuaTable && t.IsArray)
            {
                var innerT = t.GetElementType();
                var table = (LuaTable)value;
                var array = Array.CreateInstance(innerT, table.Count);
                var i = 0;
                foreach(var kv in table)
                {
                    using (kv.Key)
                    {
                        object element;
                        if (innerT == typeof(LuaValue))
                            element = kv.Value;
                        else
                        {
                            var elementHasClrValue = kv.Value.TryGetClrValue(innerT, out element);
                            if(!elementHasClrValue || !(element is LuaValue))
                            {
                                kv.Value.Dispose();
                            }
                            if (!elementHasClrValue)
                                throw new LuaException("Unable to convert table value of type {0} to type{1}".F(kv.Value.WrappedClrType(),innerT));
                        }

                        array.SetValue(element, i++);
                    }
                }
                clrObject = array;
                return true;
            }

            clrObject = t.IsValueType ? Activator.CreateInstance(t) : null;
            return false;
        }
    }

}