﻿using System;
using System.IO;
using System.IO.Compression;

namespace QE {

    public static class Compressor {

        public static byte[] Compress(byte[] data) {
            using (var ms = new MemoryStream()) {
                using (var gzip = new GZipStream(ms, CompressionLevel.Optimal))
                    gzip.Write(data, 0, data.Length);
                data = ms.ToArray();
            }
            return data;
        }

        public static byte[] Decompress(byte[] data) {
            // the trick is to read the last 4 bytes to get the length
            // gzip appends this to the array when compressing
            var lengthBuffer = new byte[4];
            Array.Copy(data, data.Length - 4, lengthBuffer, 0, 4);
            int uncompressedSize = BitConverter.ToInt32(lengthBuffer, 0);
            var buffer = new byte[uncompressedSize];
            using (var ms = new MemoryStream(data)) {
                using (var gzip = new GZipStream(ms, CompressionMode.Decompress)) {
                    gzip.Read(buffer, 0, uncompressedSize);
                }
            }
            return buffer;
        }

    }

}