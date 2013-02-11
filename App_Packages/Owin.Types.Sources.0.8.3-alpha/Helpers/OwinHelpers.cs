// <copyright file="OwinHelpers.cs" company="Microsoft Open Technologies, Inc.">
// Copyright 2013 Microsoft Open Technologies, Inc. All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Owin.Types.Helpers
{
#region OwinHelpers

    [System.CodeDom.Compiler.GeneratedCode("App_Packages", "")]
    internal static partial class OwinHelpers
    {
    }
#endregion

#region OwinHelpers.Forwarded

    internal static partial class OwinHelpers
    {
        public static string GetForwardedScheme(OwinRequest request)
        {
            var headers = request.Headers;

            var forwardedSsl = GetHeader(headers, "X-Forwarded-Ssl");
            if (forwardedSsl != null && string.Equals(forwardedSsl, "on", StringComparison.OrdinalIgnoreCase))
            {
                return "https";
            }

            var forwardedScheme = GetHeader(headers, "X-Forwarded-Scheme");
            if (!string.IsNullOrWhiteSpace(forwardedScheme))
            {
                return forwardedScheme;
            }

            var forwardedProto = GetHeaderSplit(headers, "X-Forwarded-Proto");
            if (forwardedProto != null)
            {
                forwardedScheme = forwardedProto.FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(forwardedScheme))
                {
                    return forwardedScheme;
                }
            }

            return request.Scheme;
        }

        public static string GetForwardedHost(OwinRequest request)
        {
            var headers = request.Headers;

            var forwardedHost = GetHeaderSplit(headers, "X-Forwarded-Host");
            if (forwardedHost != null)
            {
                return forwardedHost.Last();
            }

            var host = GetHeader(headers, "Host");
            if (!string.IsNullOrWhiteSpace(host))
            {
                return host;
            }

            var localIpAddress = request.LocalIpAddress ?? "localhost";
            var localPort = request.LocalPort;
            return string.IsNullOrWhiteSpace(localPort) ? localIpAddress : (localIpAddress + ":" + localPort);
        }

        public static Uri GetForwardedUri(OwinRequest request)
        {
            var queryString = request.QueryString;

            return string.IsNullOrWhiteSpace(queryString) 
                ? new Uri(GetForwardedScheme(request) + "://" + GetForwardedHost(request) + request.PathBase + request.Path) 
                : new Uri(GetForwardedScheme(request) + "://" + GetForwardedHost(request) + request.PathBase + request.Path + "?" + queryString);
        }

        public static OwinRequest ApplyForwardedScheme(OwinRequest request)
        {
            request.Scheme = GetForwardedScheme(request);
            return request;
        }

        public static OwinRequest ApplyForwardedHost(OwinRequest request)
        {
            request.Host = GetForwardedHost(request);
            return request;
        }

        public static OwinRequest ApplyForwardedUri(OwinRequest request)
        {
            return ApplyForwardedHost(ApplyForwardedScheme(request));
        }

    }
#endregion

#region OwinHelpers.Header

    internal static partial class OwinHelpers
    {
        public static string GetHeader(IDictionary<string, string[]> headers, string key)
        {
            string[] values = GetHeaderUnmodified(headers, key);
            return values == null ? null : string.Join(",", values);
        }

        public static IEnumerable<string> GetHeaderSplit(IDictionary<string, string[]> headers, string key)
        {
            string[] values = GetHeaderUnmodified(headers, key);
            return values == null ? null : GetHeaderSplitImplementation(values);
        }

        private static IEnumerable<string> GetHeaderSplitImplementation(string[] values)
        {
            foreach (var segment in new HeaderSegmentCollection(values))
            {
                if (segment.Data.HasValue)
                {
                    yield return segment.Data.Value;
                }
            }
        }

        public static string[] GetHeaderUnmodified(IDictionary<string, string[]> headers, string key)
        {
            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }
            string[] values;
            return headers.TryGetValue(key, out values) ? values : null;
        }

        public static void SetHeader(IDictionary<string, string[]> headers, string key, string value)
        {
            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }
            headers[key] = new[] { value };
        }

        public static void SetHeaderJoined(IDictionary<string, string[]> headers, string key, params string[] values)
        {
            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }
            headers[key] = new[] { string.Join(",", values) };
        }

        public static void SetHeaderJoined(IDictionary<string, string[]> headers, string key, IEnumerable<string> values)
        {
            SetHeaderJoined(headers, key, values.ToArray());
        }

        public static void SetHeaderUnmodified(IDictionary<string, string[]> headers, string key, params string[] values)
        {
            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }
            headers[key] = values;
        }

        public static void SetHeaderUnmodified(IDictionary<string, string[]> headers, string key, IEnumerable<string> values)
        {
            if (headers == null)
            {
                throw new ArgumentNullException("headers");
            }
            headers[key] = values.ToArray();
        }

        public static void AddHeader(IDictionary<string, string[]> headers, string key, string value)
        {
            AddHeaderUnmodified(headers, key, value);
        }

        public static void AddHeaderJoined(IDictionary<string, string[]> headers, string key, params string[] values)
        {
            var existing = GetHeaderUnmodified(headers, key);
            if (existing == null)
            {
                SetHeaderJoined(headers, key, values);
            }
            else
            {
                SetHeaderJoined(headers, key, existing.Concat(values));
            }
        }

        public static void AddHeaderJoined(IDictionary<string, string[]> headers, string key, IEnumerable<string> values)
        {
            var existing = GetHeaderUnmodified(headers, key);
            SetHeaderJoined(headers, key, existing == null ? values : existing.Concat(values));
        }

        public static void AddHeaderUnmodified(IDictionary<string, string[]> headers, string key, params string[] values)
        {
            var existing = GetHeaderUnmodified(headers, key);
            if (existing == null)
            {
                SetHeaderUnmodified(headers, key, values);
            }
            else
            {
                SetHeaderUnmodified(headers, key, existing.Concat(values));
            }
        }

        public static void AddHeaderUnmodified(IDictionary<string, string[]> headers, string key, IEnumerable<string> values)
        {
            var existing = GetHeaderUnmodified(headers, key);
            SetHeaderUnmodified(headers, key, existing == null ? values : existing.Concat(values));
        }
    }
#endregion

#region OwinHelpers.HeaderSegment

    [System.CodeDom.Compiler.GeneratedCode("App_Packages", "")]
    internal struct HeaderSegment : IEquatable<HeaderSegment>
    {
        private readonly StringSegment _formatting;
        private readonly StringSegment _data;

        // <summary>
        // Initializes a new instance of the <see cref="T:System.Object"/> class.
        // </summary>
        public HeaderSegment(StringSegment formatting, StringSegment data)
        {
            _formatting = formatting;
            _data = data;
        }

        public StringSegment Formatting
        {
            get { return _formatting; }
        }

        public StringSegment Data
        {
            get { return _data; }
        }

        #region Equality members

        public bool Equals(HeaderSegment other)
        {
            return _formatting.Equals(other._formatting) && _data.Equals(other._data);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is HeaderSegment && Equals((HeaderSegment)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_formatting.GetHashCode() * 397) ^ _data.GetHashCode();
            }
        }

        public static bool operator ==(HeaderSegment left, HeaderSegment right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(HeaderSegment left, HeaderSegment right)
        {
            return !left.Equals(right);
        }

        #endregion

    }
#endregion

#region OwinHelpers.HeaderSegmentCollection

    [System.CodeDom.Compiler.GeneratedCode("App_Packages", "")]
    internal struct HeaderSegmentCollection : IEnumerable<HeaderSegment>, IEquatable<HeaderSegmentCollection>
    {
        private readonly string[] _headers;

        public HeaderSegmentCollection(string[] headers)
        {
            _headers = headers;
        }

        #region Equality members

        public bool Equals(HeaderSegmentCollection other)
        {
            return Equals(_headers, other._headers);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is HeaderSegmentCollection && Equals((HeaderSegmentCollection)obj);
        }

        public override int GetHashCode()
        {
            return (_headers != null ? _headers.GetHashCode() : 0);
        }

        public static bool operator ==(HeaderSegmentCollection left, HeaderSegmentCollection right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(HeaderSegmentCollection left, HeaderSegmentCollection right)
        {
            return !left.Equals(right);
        }

        #endregion

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_headers);
        }

        IEnumerator<HeaderSegment> IEnumerable<HeaderSegment>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal struct Enumerator : IEnumerator<HeaderSegment>
        {
            private enum Mode
            {
                Leading,
                Value,
                ValueQuoted,
                Trailing,
                Produce,
            }
            private enum Attr
            {
                Value,
                Quote,
                Delimiter,
                Whitespace
            }

            private readonly string[] _headers;
            private int _index;

            private string _header;
            private int _headerLength;
            private int _offset;

            private int _leadingStart;
            private int _leadingEnd;
            private int _valueStart;
            private int _valueEnd;
            private int _trailingStart;

            private Mode _mode;

            private static readonly string[] NoHeaders = new string[0];

            public Enumerator(string[] headers)
            {
                _headers = headers ?? NoHeaders;
                _header = string.Empty;
                _headerLength = -1;
                _index = -1;
                _offset = -1;
                _leadingStart = -1;
                _leadingEnd = -1;
                _valueStart = -1;
                _valueEnd = -1;
                _trailingStart = -1;
                _mode = Mode.Leading;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                for (; ; )
                {
                    if (_mode == Mode.Produce)
                    {
                        _leadingStart = _trailingStart;
                        _leadingEnd = -1;
                        _valueStart = -1;
                        _valueEnd = -1;
                        _trailingStart = -1;

                        if (_offset == _headerLength &&
                            _leadingStart != -1 &&
                            _leadingStart != _offset)
                        {
                            // Also produce trailing whitespace
                            _leadingEnd = _offset;
                            return true;
                        }
                        _mode = Mode.Leading;
                    }

                    // if end of a string
                    if (_offset == _headerLength)
                    {
                        ++_index;
                        _offset = -1;
                        _leadingStart = 0;
                        _leadingEnd = -1;
                        _valueStart = -1;
                        _valueEnd = -1;
                        _trailingStart = -1;

                        // if that was the last string
                        if (_index == _headers.Length)
                        {
                            // no more move nexts
                            return false;
                        }

                        // grab the next string
                        _header = _headers[_index] ?? string.Empty;
                        _headerLength = _header.Length;
                    }
                    for (; ; )
                    {
                        ++_offset;
                        var ch = _offset == _headerLength ? (char)0 : _header[_offset];
                        // todo - array of attrs
                        var attr = char.IsWhiteSpace(ch) ? Attr.Whitespace : ch == '\"' ? Attr.Quote : (ch == ',' || ch == (char)0) ? Attr.Delimiter : Attr.Value;

                        switch (_mode)
                        {
                            case Mode.Leading:
                                switch (attr)
                                {
                                    case Attr.Delimiter:
                                        _leadingEnd = _offset;
                                        _mode = Mode.Produce;
                                        break;
                                    case Attr.Quote:
                                        _leadingEnd = _offset;
                                        _valueStart = _offset;
                                        _mode = Mode.ValueQuoted;
                                        break;
                                    case Attr.Value:
                                        _leadingEnd = _offset;
                                        _valueStart = _offset;
                                        _mode = Mode.Value;
                                        break;
                                    case Attr.Whitespace:
                                        // more
                                        break;
                                }
                                break;
                            case Mode.Value:
                                switch (attr)
                                {
                                    case Attr.Quote:
                                        _mode = Mode.ValueQuoted;
                                        break;
                                    case Attr.Delimiter:
                                        _valueEnd = _offset;
                                        _trailingStart = _offset;
                                        _mode = Mode.Produce;
                                        break;
                                    case Attr.Value:
                                        // more
                                        break;
                                    case Attr.Whitespace:
                                        _valueEnd = _offset;
                                        _trailingStart = _offset;
                                        _mode = Mode.Trailing;
                                        break;
                                }
                                break;
                            case Mode.ValueQuoted:
                                switch (attr)
                                {
                                    case Attr.Quote:
                                        _mode = Mode.Value;
                                        break;
                                    case Attr.Delimiter:
                                        if (ch == (char)0)
                                        {
                                            _valueEnd = _offset;
                                            _trailingStart = _offset;
                                            _mode = Mode.Produce;
                                        }
                                        break;
                                    case Attr.Value:
                                    case Attr.Whitespace:
                                        // more
                                        break;
                                }
                                break;
                            case Mode.Trailing:
                                switch (attr)
                                {
                                    case Attr.Delimiter:
                                        _mode = Mode.Produce;
                                        break;
                                    case Attr.Quote:
                                        // back into value
                                        _trailingStart = -1;
                                        _valueEnd = -1;
                                        _mode = Mode.ValueQuoted;
                                        break;
                                    case Attr.Value:
                                        // back into value
                                        _trailingStart = -1;
                                        _valueEnd = -1;
                                        _mode = Mode.Value;
                                        break;
                                    case Attr.Whitespace:
                                        // more
                                        break;
                                }
                                break;
                        }
                        if (_mode == Mode.Produce)
                        {
                            return true;
                        }
                    }
                }
            }

            public void Reset()
            {
                _index = 0;
                _offset = 0;
                _leadingStart = 0;
                _leadingEnd = 0;
                _valueStart = 0;
                _valueEnd = 0;
            }

            public HeaderSegment Current
            {
                get
                {
                    return new HeaderSegment(
                        new StringSegment(_header, _leadingStart, _leadingEnd - _leadingStart),
                        new StringSegment(_header, _valueStart, _valueEnd - _valueStart));
                }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
#endregion

#region OwinHelpers.MethodOverride

    internal static partial class OwinHelpers
    {
        public static string GetMethodOverride(OwinRequest request)
        {
            var method = request.Method;
            if (!string.Equals("POST", method, StringComparison.OrdinalIgnoreCase))
            {
                // override has no effect on POST 
                return method;
            }

            var methodOverride = GetHeader(request.Headers, "X-Http-Method-Override");
            if (string.IsNullOrEmpty(methodOverride))
            {
                return method;
            }

            return methodOverride;
        }

        public static OwinRequest ApplyMethodOverride(OwinRequest request)
        {
            request.Method = GetMethodOverride(request);
            return request;
        }
    }
#endregion

#region OwinHelpers.StringSegment

    [System.CodeDom.Compiler.GeneratedCode("App_Packages", "")]
    internal struct StringSegment : IEquatable<StringSegment>
    {
        private readonly string _buffer;
        private readonly int _offset;
        private readonly int _count;

        // <summary>
        // Initializes a new instance of the <see cref="T:System.Object"/> class.
        // </summary>
        public StringSegment(string buffer, int offset, int count)
        {
            _buffer = buffer;
            _offset = offset;
            _count = count;
        }

        public string Buffer
        {
            get { return _buffer; }
        }

        public int Offset
        {
            get { return _offset; }
        }

        public int Count
        {
            get { return _count; }
        }

        public string Value
        {
            get
            {
                return _offset == -1 ? null : _buffer.Substring(_offset, _count);
            }
        }

        public bool HasValue
        {
            get
            {
                return _offset != -1 && _count != 0 && _buffer != null;
            }
        }

        #region Equality members

        public bool Equals(StringSegment other)
        {
            return string.Equals(_buffer, other._buffer) && _offset == other._offset && _count == other._count;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is StringSegment && Equals((StringSegment)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (_buffer != null ? _buffer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _offset;
                hashCode = (hashCode * 397) ^ _count;
                return hashCode;
            }
        }

        public static bool operator ==(StringSegment left, StringSegment right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(StringSegment left, StringSegment right)
        {
            return !left.Equals(right);
        }

        #endregion

        public bool StartsWith(string text, StringComparison comparisonType)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            var textLength = text.Length;
            if (!HasValue || _count < textLength) return false;
            return string.Compare(_buffer, _offset, text, 0, textLength, comparisonType) == 0;
        }

        public bool EndsWith(string text, StringComparison comparisonType)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            var textLength = text.Length;
            if (!HasValue || _count < textLength) return false;
            return string.Compare(_buffer, _offset + _count - textLength, text, 0, textLength, comparisonType) == 0;
        }

        public bool Equals(string text, StringComparison comparisonType)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            var textLength = text.Length;
            if (!HasValue || _count != textLength) return false;
            return string.Compare(_buffer, _offset, text, 0, textLength, comparisonType) == 0;
        }

        public string Substring(int offset, int length)
        {
            return _buffer.Substring(_offset + offset, length);
        }

        public StringSegment Subsegment(int offset, int length)
        {
            return new StringSegment(_buffer, _offset + offset, length);
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }
    }
#endregion

#region OwinHelpers.Uri

    internal static partial class OwinHelpers
    {
        public static string GetHost(OwinRequest request)
        {
            var headers = request.Headers;

            var host = GetHeader(headers, "Host");
            if (!string.IsNullOrWhiteSpace(host))
            {
                return host;
            }

            var localIpAddress = request.LocalIpAddress ?? "localhost";
            var localPort = request.LocalPort;
            return string.IsNullOrWhiteSpace(localPort) ? localIpAddress : (localIpAddress + ":" + localPort);
        }

        public static Uri GetUri(OwinRequest request)
        {
            var queryString = request.QueryString;

            return string.IsNullOrWhiteSpace(queryString)
                ? new Uri(request.Scheme + "://" + GetHost(request) + request.PathBase + request.Path)
                : new Uri(request.Scheme + "://" + GetHost(request) + request.PathBase + request.Path + "?" + queryString);
        }
    }
#endregion

}
