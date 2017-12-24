﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ZTImage.Json
{
    /// <summary> 以非安全方式访问指针操作字符串直接写入内存,以提高字符串拼接效率
    /// </summary>
    [DebuggerDisplay("长度:{Length} 内容: {DebugInfo}")]
    public unsafe class UnsafeStringWriter
    {
        private const int _CACHE_LEN = 4096;
        /// <summary> 新建 以非安全方式访问指针操作字符串直接写入内存的 对象
        /// </summary>
        public UnsafeStringWriter() {
                p = (char*)System.Runtime.InteropServices.Marshal.AllocHGlobal(_CACHE_LEN * 2);
                _EndPosition = _CACHE_LEN - 1;
        }

        #region 字段
        /// <summary> 一级缓冲指针
        /// </summary>
        Char* p;
        /// <summary> 二级缓冲
        /// </summary>
        string[] _Buffer = new string[8];
        /// <summary> 备用二级缓冲索引
        /// </summary>
        int _BufferIndex = 0;
        /// <summary> 总字符数
        /// </summary>
        int _Length = 0;
        /// <summary> 结束位,一级缓冲长度减一
        /// </summary>
        int _EndPosition;
        /// <summary> 以及缓冲当前位置
        /// </summary>
        int _Position;
        #endregion

        /// <summary> 获取当前实例中的字符串总长度
        /// </summary>
        public int Length
        {
            get
            {
                return _Length + _Position;
            }
        } 

        #region 私有方法
        /// <summary> 在调试器的变量窗口中的显示的信息
        /// </summary>
        private string DebugInfo
        {
            get
            {
                string str = this.ToString();
                if (str.Length > 70)
                {
                    var s = str;
                    str = s.Substring(0, 30) + "  ......  ";
                    str += s.Substring(s.Length - 30);
                }
                return str;
            }
        }
        /// <summary> 尝试在一级缓冲区写入一个字符
        /// <para>如果一级缓冲区已满,将会自动调用Flush方法转移一级缓冲区中的内容</para>
        /// </summary>
        private void TryWrite()
        {
            if (_Position > _EndPosition)
            {
                Flush();
            }
            else if (_EndPosition == int.MaxValue)
            {
                throw new Exception("指针尚未准备就绪!");
            }
        }
        /// <summary> 尝试在一级缓冲区写入指定数量的字符
        /// </summary>
        /// <para>如果尝试写入的字符数大于一级缓冲区的大小,返回false</para>
        /// <para>如果尝试写入的字符数超出一级缓冲区剩余容量,自动调用Flush方法</para>
        /// <param name="count">尝试写入的字符数</param>
        /// <returns></returns>
        private bool TryWrite(int count)
        {
            if (count >= _EndPosition)
            {
                return false;
            }
            var pre = _Position + count;
            if (pre >= _EndPosition)
            {
                Flush();
            }
            else if (_EndPosition == int.MaxValue)
            {
                throw new Exception("指针尚未准备就绪!");
            }
            return true;
        } 
        #endregion

        #region Append
        /// <summary> 将 Boolean 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(Boolean val)
        {
            if (val)
            {
                TryWrite(4);
                p[_Position++] = 't';
                p[_Position++] = 'r';
                p[_Position++] = 'u';
                p[_Position++] = 'e';
            }
            else
            {
                TryWrite(5);
                p[_Position++] = 'f';
                p[_Position++] = 'a';
                p[_Position++] = 'l';
                p[_Position++] = 's';
                p[_Position++] = 'e';
            }
            return this;
        }
        /// <summary> 将 DateTime 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(DateTime val)
        {
            TryWrite(18);
            int a = val.Year;
            #region 年
            if (a > 999)
            {
                p[_Position++] = (char)(a / 1000 + '0');
                a = a % 1000;
                p[_Position++] = (char)(a / 100 + '0');
                a = a % 100;
                p[_Position++] = (char)(a / 10 + '0');
                a = a % 10;
            }
            else if (a > 99)
            {
                p[_Position++] = '0';
                p[_Position++] = (char)(a / 100 + '0');
                a = a % 100;
                p[_Position++] = (char)(a / 10 + '0');
                a = a % 10;
            }
            else if (a > 9)
            {
                p[_Position++] = '0';
                p[_Position++] = '0';
                p[_Position++] = (char)(a / 10 + '0');
                a = a % 10;
            }
            else
            {
                p[_Position++] = '0';
                p[_Position++] = '0';
                p[_Position++] = '0';
            }

            p[_Position++] = (char)(a + '0');
            #endregion
            p[_Position++] = '-';
            a = val.Month;
            #region 月
            if (a > 9)
            {
                p[_Position++] = (char)(a / 10 + '0');
                a = a % 10;
            }
            else
            {
                p[_Position++] = '0';
            }
            p[_Position++] = (char)(a + '0');
            #endregion
            a = val.Day;
            p[_Position++] = '-';
            #region 日
            if (a > 9)
            {
                p[_Position++] = (char)(a / 10 + '0');
                a = a % 10;
            }
            else
            {
                p[_Position++] = '0';
            }
            p[_Position++] = (char)(a + '0');
            #endregion
            a = val.Hour;
            p[_Position++] = ' ';
            #region 时
            if (a > 9)
            {
                p[_Position++] = (char)(a / 10 + '0');
                a = a % 10;
            }
            else
            {
                p[_Position++] = '0';
            }
            p[_Position++] = (char)(a + '0');
            #endregion
            a = val.Minute;
            p[_Position++] = ':';
            #region 分
            if (a > 9)
            {
                p[_Position++] = (char)(a / 10 + '0');
                a = a % 10;
            }
            else
            {
                p[_Position++] = '0';
            }
            p[_Position++] = (char)(a + '0');
            #endregion
            a = val.Second;
            p[_Position++] = ':';
            #region 秒
            if (a > 9)
            {
                p[_Position++] = (char)(a / 10 + '0');
                a = a % 10;
            }
            else
            {
                p[_Position++] = '0';
            }
            p[_Position++] = (char)(a + '0');
            #endregion
            return this;
        }
        /// <summary> 将 Guid 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(Guid val)
        {
            Append(val.ToString());
            return this;
        }
        /// <summary> 将 Decimal 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(Decimal val)
        {
            Append(val.ToString());
            return this;
        }
        /// <summary> 将 Double 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(Double val)
        {
            Append(Convert.ToString(val));
            return this;
        }
        /// <summary> 将 Single 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(Single val)
        {
            Append(Convert.ToString(val));
            return this;
        }
        /// <summary> 将 SByte 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(SByte val)
        {
            Append((Int64)val);
            return this;
        }
        /// <summary> 将 Int16 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(Int16 val)
        {
            Append((Int64)val);
            return this;
        }
        /// <summary> 将 Int32 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(Int32 val)
        {
            Append((Int64)val);
            return this;
        }
        /// <summary> 将 Int64 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(Int64 val)
        {
            
            
            if (val == 0)
            {
                p[_Position++] = '0';
                return this;
            }

            char* number = stackalloc char[64];

            var pos = 63;
            if (val < 0)
            {
                p[_Position++] = '-';
                number[pos] = (char)(~(val % 10) + '1');
                if (val < -10)
                {
                    val = val / -10;
                    number[--pos] = (char)(val % 10 + '0');
                }
            }
            else
            {
                number[pos] = (char)(val % 10 + '0');
            }
            while ((val = val / 10L) != 0L)
            {
                number[--pos] = (char)(val % 10L + '0');
            }
            var length = 64 - pos;
            TryWrite(length);
            char* p1 = (char*)&p[_Position];
            char* p2 = ((char*)&number[pos]);
            _Position += length;
            while (length > 0)
            {
                *p1 = *p2;
                p1++;
                p2++;
                length--;
            }
            //while (length >= 4)
            //{
            //    (*p1++) = *(p2++);
            //    (*p1++) = *(p2++);
            //    length -= 4;
            //}
            //if ((length & 2) != 0)
            //{
            //    (*p1++) = *(p2++);
            //}
            //if ((length & 1) != 0)
            //{
            //    *(char*)p1 = number[pos];
            //}
            return this;
        }
        /// <summary> 将 Char 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(Char val)
        {
            TryWrite();
            p[_Position++] = val;
            return this;
        }
        /// <summary> 将 Byte 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(Byte val)
        {
            Append((UInt64)val);
            return this;
        }
        /// <summary> 将 UInt16 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(UInt16 val)
        {
            Append((UInt64)val);
            return this;
        }
        /// <summary> 将 UInt32 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(UInt32 val)
        {
            Append((UInt64)val);
            return this;
        }
        /// <summary> 将 UInt64 对象转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(UInt64 val)
        {
            if (val == 0)
            {
                p[_Position++] = '0';
                return this;
            }
            char* number = stackalloc char[64];
            var pos = 63;

            number[pos] = (char)(val % 10 + '0');

            while ((val = val / 10L) != 0L)
            {
                number[--pos] = (char)(val % 10L + '0');
            }
            var length = 64 - pos;
            TryWrite(length);

            while (pos < 60)
            {
                ((int*)p)[_Position += 2] = ((int*)number)[pos += 2];
                ((int*)p)[_Position += 2] = ((int*)number)[pos += 2];
            }
            if (pos < 62)
            {
                ((int*)p)[_Position += 2] = ((int*)number)[pos += 2];
            }
            if (pos < 63)
            {
                p[_Position++] = number[63];
            }
            return this;
        }
        /// <summary> 将字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter Append(String val)
        {
            var length = val.Length;
            if (val == null || length == 0)
            {
                return this;
            }
            else if (length <= 3)
            {
                TryWrite(length);
                p[_Position++] = val[0];
                if (length > 1)
                {
                    p[_Position++] = val[1];
                    if (length > 2)
                    {
                        p[_Position++] = val[2];
                    }
                }
            }
            else if (TryWrite(length))
            {
                fixed (char* c = val)
                {
                    int* p2;
                    if ((length & 1) != 0)
                    {
                        p[_Position++] = c[0];
                        p2 = ((int*)(c+1));
                        length--;
                    }
                    else
                    {
                        p2 = ((int*)c);
                    }
                    int* p1 = (int*)&p[_Position];
                    
                    
                    _Position += length;
                    while (length >= 8)
                    {
                        (*p1++) = *(p2++);
                        (*p1++) = *(p2++);
                        (*p1++) = *(p2++);
                        (*p1++) = *(p2++);
                        length -= 8;
                    }
                    if ((length & 4) != 0)
                    {
                        (*p1++) = *(p2++);
                        (*p1++) = *(p2++);
                    }
                    if ((length & 2) != 0)
                    {
                        (*p1++) = *(p2++);
                    }
                }
            }
            else
            {
                Flush();
                _Buffer[_BufferIndex++] = val;
                _Length += val.Length;
            }
            return this;
        }
        /// <summary> 将内存中的字符串追加到当前实例。
        /// </summary>
        /// <param name="point">内存指针</param>
        /// <param name="offset">指针偏移量</param>
        /// <param name="length">字符长度</param>
        /// <returns></returns>
        internal UnsafeStringWriter Append(char* point, int offset, int length)
        {
            if (length > 0)
            {
                if (TryWrite(length))
                {
                    char* c = point + offset;
                    if ((length & 1) != 0)
                    {
                        p[_Position++] = c[0];
                        c++;
                        length--;
                    }
                    int* p1 = (int*)&p[_Position];
                    int* p2 = ((int*)c);
                    _Position += length;
                    while (length >= 8)
                    {
                        (*p1++) = *(p2++);
                        (*p1++) = *(p2++);
                        (*p1++) = *(p2++);
                        (*p1++) = *(p2++);
                        length -= 8;
                    }
                    if ((length & 4) != 0)
                    {
                        (*p1++) = *(p2++);
                        (*p1++) = *(p2++);
                    }
                    if ((length & 2) != 0)
                    {
                        (*p1++) = *(p2++);
                    }
                }
                else
                {
                    Flush();
                    _Buffer[_BufferIndex++] = new string(point, offset, length);
                    _Length += length;
                }
            }

            return this;
        }
        /// <summary> 将可格式化对象,按指定的格式转换为字符串追加到当前实例。
        /// </summary>
        public UnsafeStringWriter AppendFormat(IFormattable val, string format)
        {
            Append(val.ToString(format, null));
            return this;
        }
        #endregion

        #region IDispone
        /// <summary> 关闭当前实例
        /// <para>
        /// 该行为将清空所有缓冲区中的内容,
        /// 并阻止除Ready,Close以外的方法调用,直到再次调用Ready方法
        /// </para>
        /// </summary>
        public void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_disposed) return;
            if (isDisposing)
            {
                if (p != null)
                {
                    System.Runtime.InteropServices.Marshal.FreeHGlobal((IntPtr)p);
                    _Buffer[0] = _Buffer[1] =
                    _Buffer[2] = _Buffer[3] =
                    _Buffer[4] = _Buffer[5] =
                    _Buffer[6] = _Buffer[7] = null;
                    _Length = 0;
                    _Position = 0;
                    _EndPosition = int.MaxValue;
                    p = null;
                }
                
            }
            _disposed = true;
        }

        ~UnsafeStringWriter()
        {
            Dispose(false);
        }
        #endregion

        /// <summary> 清理当前实例的一级缓冲区的内容，使所有缓冲数据写入二级缓冲区。
        /// </summary>
        public void Flush()
        {
            if (_Position > 0)
            {
                _Length += _Position;
                if (_BufferIndex == 8)
                {
                    _Buffer[0] = string.Concat(_Buffer[0], _Buffer[1], _Buffer[2], _Buffer[3]);
                    _Buffer[1] = string.Concat(_Buffer[4], _Buffer[5], _Buffer[6], _Buffer[7]);
                    _Buffer[2] = new string(p, 0, _Position);
                    _Buffer[3] =
                    _Buffer[4] =
                    _Buffer[5] =
                    _Buffer[6] =
                    _Buffer[7] = null;
                    _BufferIndex = 3;
                }
                else
                {
                    _Buffer[_BufferIndex++] = new string(p, 0, _Position);
                }
                _Position = 0;
            }
        }

        /// <summary> 返回当前实例中的字符串
        /// </summary>
        public override string ToString()
        {
            if (_Length == 0)
            {
                return new string(p, 0, _Position);
            }
            else if (_Length <= 4)
            {
                return string.Concat(_Buffer[0], _Buffer[1], _Buffer[2], new string(p, 0, _Position));
            }
            else
            {
                return string.Concat(
                    _Buffer[0], _Buffer[1], _Buffer[2], _Buffer[3],
                    _Buffer[4], _Buffer[5], _Buffer[6], _Buffer[7],
                    new string(p, 0, _Position));
            }
        }

    }
}
