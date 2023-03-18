using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x0200055A RID: 1370
public class lzip
{
    // Token: 0x06002E93 RID: 11923
    [DllImport("zipw", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int zsetPermissions(string filePath, string _user, string _group, string _other);

    // Token: 0x06002E94 RID: 11924
    [DllImport("zipw")]
    internal static extern bool zipValidateFile(string zipArchive, IntPtr FileBuffer, int fileBufferLength);

    // Token: 0x06002E95 RID: 11925
    [DllImport("zipw")]
    internal static extern int zipGetTotalFiles(string zipArchive, IntPtr FileBuffer, int fileBufferLength);

    // Token: 0x06002E96 RID: 11926
    [DllImport("zipw")]
    internal static extern int zipGetTotalEntries(string zipArchive, IntPtr FileBuffer, int fileBufferLength);

    // Token: 0x06002E97 RID: 11927
    [DllImport("zipw")]
    internal static extern int zipGetInfoA(string zipArchive, IntPtr total, IntPtr FileBuffer, int fileBufferLength);

    // Token: 0x06002E98 RID: 11928
    [DllImport("zipw")]
    internal static extern IntPtr zipGetInfo(string zipArchive, int size, IntPtr unc, IntPtr comp, IntPtr FileBuffer, int fileBufferLength);

    // Token: 0x06002E99 RID: 11929
    [DllImport("zipw")]
    internal static extern void releaseBuffer(IntPtr buffer);

    // Token: 0x06002E9A RID: 11930
    [DllImport("zipw")]
    internal static extern int zipGetEntrySize(string zipArchive, string entry, IntPtr FileBuffer, int fileBufferLength);

    // Token: 0x06002E9B RID: 11931
    [DllImport("zipw")]
    internal static extern bool zipEntryExists(string zipArchive, string entry, IntPtr FileBuffer, int fileBufferLength);

    // Token: 0x06002E9C RID: 11932
    [DllImport("zipw")]
    internal static extern int zipCD(int levelOfCompression, string zipArchive, string inFilePath, string fileName, string comment, [MarshalAs(UnmanagedType.LPStr)] string password, bool useBz2);

    // Token: 0x06002E9D RID: 11933
    [DllImport("zipw")]
    internal static extern bool zipBuf2File(int levelOfCompression, string zipArchive, string arc_filename, IntPtr buffer, int bufferSize, string comment, [MarshalAs(UnmanagedType.LPStr)] string password, bool useBz2);

    // Token: 0x06002E9E RID: 11934
    [DllImport("zipw")]
    internal static extern int zipDeleteFile(string zipArchive, string arc_filename, string tempArchive);

    // Token: 0x06002E9F RID: 11935
    [DllImport("zipw")]
    internal static extern int zipEntry2Buffer(string zipArchive, string entry, IntPtr buffer, int bufferSize, IntPtr FileBuffer, int fileBufferLength, [MarshalAs(UnmanagedType.LPStr)] string password);

    // Token: 0x06002EA0 RID: 11936
    [DllImport("zipw")]
    internal static extern IntPtr zipCompressBuffer(IntPtr source, int sourceLen, int levelOfCompression, ref int v);

    // Token: 0x06002EA1 RID: 11937
    [DllImport("zipw")]
    internal static extern IntPtr zipDecompressBuffer(IntPtr source, int sourceLen, ref int v);

    // Token: 0x06002EA2 RID: 11938
    [DllImport("zipw")]
    internal static extern int zipEX(string zipArchive, string outPath, IntPtr progress, IntPtr FileBuffer, int fileBufferLength, IntPtr proc, [MarshalAs(UnmanagedType.LPStr)] string password);

    // Token: 0x06002EA3 RID: 11939
    [DllImport("zipw")]
    internal static extern int zipEntry(string zipArchive, string arc_filename, string outpath, IntPtr FileBuffer, int fileBufferLength, IntPtr proc, [MarshalAs(UnmanagedType.LPStr)] string password);

    // Token: 0x06002EA4 RID: 11940
    [DllImport("zipw")]
    internal static extern uint getEntryDateTime(string zipArchive, string arc_filename, IntPtr FileBuffer, int fileBufferLength);

    // Token: 0x06002EA5 RID: 11941
    [DllImport("zipw")]
    internal static extern int zipGzip(IntPtr source, int sourceLen, IntPtr outBuffer, int levelOfCompression, bool addHeader, bool addFooter);

    // Token: 0x06002EA6 RID: 11942
    [DllImport("zipw")]
    internal static extern int zipUnGzip(IntPtr source, int sourceLen, IntPtr outBuffer, int outLen, bool hasHeader, bool hasFooter);

    // Token: 0x06002EA7 RID: 11943
    [DllImport("zipw")]
    internal static extern int zipUnGzip2(IntPtr source, int sourceLen, IntPtr outBuffer, int outLen);

    // Token: 0x06002EA8 RID: 11944 RVA: 0x00020635 File Offset: 0x0001E835
    public static int setFilePermissions(string filePath, string _user, string _group, string _other)
    {
        return lzip.zsetPermissions(filePath, _user, _group, _other);
    }

    // Token: 0x06002EA9 RID: 11945 RVA: 0x0010CCCC File Offset: 0x0010AECC
    public static bool validateFile(string zipArchive, byte[] FileBuffer = null)
    {
        if (FileBuffer != null)
        {
            GCHandle gchandle = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            bool result = lzip.zipValidateFile(null, gchandle.AddrOfPinnedObject(), FileBuffer.Length);
            gchandle.Free();
            return result;
        }
        return lzip.zipValidateFile(zipArchive, IntPtr.Zero, 0);
    }

    // Token: 0x06002EAA RID: 11946 RVA: 0x0010CD14 File Offset: 0x0010AF14
    public static int getTotalFiles(string zipArchive, byte[] FileBuffer = null)
    {
        if (FileBuffer != null)
        {
            GCHandle gchandle = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            int result = lzip.zipGetTotalFiles(null, gchandle.AddrOfPinnedObject(), FileBuffer.Length);
            gchandle.Free();
            return result;
        }
        return lzip.zipGetTotalFiles(zipArchive, IntPtr.Zero, 0);
    }

    // Token: 0x06002EAB RID: 11947 RVA: 0x0010CD58 File Offset: 0x0010AF58
    public static int getTotalEntries(string zipArchive, byte[] FileBuffer = null)
    {
        if (FileBuffer != null)
        {
            GCHandle gchandle = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            int result = lzip.zipGetTotalEntries(null, gchandle.AddrOfPinnedObject(), FileBuffer.Length);
            gchandle.Free();
            return result;
        }
        return lzip.zipGetTotalEntries(zipArchive, IntPtr.Zero, 0);
    }

    // Token: 0x06002EAC RID: 11948 RVA: 0x0010CD9C File Offset: 0x0010AF9C
    public static long getFileInfo(string zipArchive, string path = null, byte[] FileBuffer = null)
    {
        lzip.ninfo.Clear();
        lzip.uinfo.Clear();
        lzip.cinfo.Clear();
        lzip.zipFiles = 0;
        lzip.zipFolders = 0;
        int[] array = new int[1];
        GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
        int num;
        if (FileBuffer != null)
        {
            GCHandle gchandle2 = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            num = lzip.zipGetInfoA(null, gchandle.AddrOfPinnedObject(), gchandle2.AddrOfPinnedObject(), FileBuffer.Length);
            gchandle2.Free();
        }
        else
        {
            num = lzip.zipGetInfoA(zipArchive, gchandle.AddrOfPinnedObject(), IntPtr.Zero, 0);
        }
        gchandle.Free();
        if (num <= 0)
        {
            return -1L;
        }
        IntPtr intPtr = IntPtr.Zero;
        uint[] array2 = new uint[array[0]];
        uint[] array3 = new uint[array[0]];
        GCHandle gchandle3 = GCHandle.Alloc(array2, GCHandleType.Pinned);
        GCHandle gchandle4 = GCHandle.Alloc(array3, GCHandleType.Pinned);
        if (FileBuffer != null)
        {
            GCHandle gchandle5 = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            intPtr = lzip.zipGetInfo(null, num, gchandle3.AddrOfPinnedObject(), gchandle4.AddrOfPinnedObject(), gchandle5.AddrOfPinnedObject(), FileBuffer.Length);
            gchandle5.Free();
        }
        else
        {
            intPtr = lzip.zipGetInfo(zipArchive, num, gchandle3.AddrOfPinnedObject(), gchandle4.AddrOfPinnedObject(), IntPtr.Zero, 0);
        }
        if (intPtr == IntPtr.Zero)
        {
            gchandle3.Free();
            gchandle4.Free();
            return -2L;
        }
        string s = Marshal.PtrToStringAuto(intPtr);
        StringReader stringReader = new StringReader(s);
        long num2 = 0L;
        for (int i = 0; i < array[0]; i++)
        {
            string item;
            if ((item = stringReader.ReadLine()) != null)
            {
                lzip.ninfo.Add(item);
            }
            if (array2 != null)
            {
                lzip.uinfo.Add((long)((ulong)array2[i]));
                num2 += (long)((ulong)array2[i]);
                if (array2[i] > 0U)
                {
                    lzip.zipFiles++;
                }
                else
                {
                    lzip.zipFolders++;
                }
            }
            if (array3 != null)
            {
                lzip.cinfo.Add((long)((ulong)array3[i]));
            }
        }
        stringReader.Close();
        stringReader.Dispose();
        gchandle3.Free();
        gchandle4.Free();
        lzip.releaseBuffer(intPtr);
        return num2;
    }

    // Token: 0x06002EAD RID: 11949 RVA: 0x0010CFBC File Offset: 0x0010B1BC
    public static int getEntrySize(string zipArchive, string entry, byte[] FileBuffer = null)
    {
        if (FileBuffer != null)
        {
            GCHandle gchandle = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            int result = lzip.zipGetEntrySize(null, entry, gchandle.AddrOfPinnedObject(), FileBuffer.Length);
            gchandle.Free();
            return result;
        }
        return lzip.zipGetEntrySize(zipArchive, entry, IntPtr.Zero, 0);
    }

    // Token: 0x06002EAE RID: 11950 RVA: 0x0010D004 File Offset: 0x0010B204
    public static bool entryExists(string zipArchive, string entry, byte[] FileBuffer = null)
    {
        if (FileBuffer != null)
        {
            GCHandle gchandle = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            bool result = lzip.zipEntryExists(null, entry, gchandle.AddrOfPinnedObject(), FileBuffer.Length);
            gchandle.Free();
            return result;
        }
        return lzip.zipEntryExists(zipArchive, entry, IntPtr.Zero, 0);
    }

    // Token: 0x06002EAF RID: 11951 RVA: 0x0010D04C File Offset: 0x0010B24C
    public static bool compressBuffer(byte[] source, ref byte[] outBuffer, int levelOfCompression)
    {
        if (levelOfCompression < 0)
        {
            levelOfCompression = 0;
        }
        if (levelOfCompression > 10)
        {
            levelOfCompression = 10;
        }
        GCHandle gchandle = GCHandle.Alloc(source, GCHandleType.Pinned);
        int num = 0;
        IntPtr intPtr = lzip.zipCompressBuffer(gchandle.AddrOfPinnedObject(), source.Length, levelOfCompression, ref num);
        if (num == 0 || intPtr == IntPtr.Zero)
        {
            gchandle.Free();
            lzip.releaseBuffer(intPtr);
            return false;
        }
        Array.Resize<byte>(ref outBuffer, num);
        Marshal.Copy(intPtr, outBuffer, 0, num);
        gchandle.Free();
        lzip.releaseBuffer(intPtr);
        return true;
    }

    // Token: 0x06002EB0 RID: 11952 RVA: 0x0010D0D0 File Offset: 0x0010B2D0
    public static int compressBufferFixed(byte[] source, ref byte[] outBuffer, int levelOfCompression, bool safe = true)
    {
        if (levelOfCompression < 0)
        {
            levelOfCompression = 0;
        }
        if (levelOfCompression > 10)
        {
            levelOfCompression = 10;
        }
        GCHandle gchandle = GCHandle.Alloc(source, GCHandleType.Pinned);
        int num = 0;
        IntPtr intPtr = lzip.zipCompressBuffer(gchandle.AddrOfPinnedObject(), source.Length, levelOfCompression, ref num);
        if (num == 0 || intPtr == IntPtr.Zero)
        {
            gchandle.Free();
            lzip.releaseBuffer(intPtr);
            return 0;
        }
        if (num > outBuffer.Length)
        {
            if (safe)
            {
                gchandle.Free();
                lzip.releaseBuffer(intPtr);
                return 0;
            }
            num = outBuffer.Length;
        }
        Marshal.Copy(intPtr, outBuffer, 0, num);
        gchandle.Free();
        lzip.releaseBuffer(intPtr);
        return num;
    }

    // Token: 0x06002EB1 RID: 11953 RVA: 0x0010D174 File Offset: 0x0010B374
    public static byte[] compressBuffer(byte[] source, int levelOfCompression)
    {
        if (levelOfCompression < 0)
        {
            levelOfCompression = 0;
        }
        if (levelOfCompression > 10)
        {
            levelOfCompression = 10;
        }
        GCHandle gchandle = GCHandle.Alloc(source, GCHandleType.Pinned);
        int num = 0;
        IntPtr intPtr = lzip.zipCompressBuffer(gchandle.AddrOfPinnedObject(), source.Length, levelOfCompression, ref num);
        if (num == 0 || intPtr == IntPtr.Zero)
        {
            gchandle.Free();
            lzip.releaseBuffer(intPtr);
            return null;
        }
        byte[] array = new byte[num];
        Marshal.Copy(intPtr, array, 0, num);
        gchandle.Free();
        lzip.releaseBuffer(intPtr);
        return array;
    }

    // Token: 0x06002EB2 RID: 11954 RVA: 0x0010D1F8 File Offset: 0x0010B3F8
    public static bool decompressBuffer(byte[] source, ref byte[] outBuffer)
    {
        GCHandle gchandle = GCHandle.Alloc(source, GCHandleType.Pinned);
        int num = 0;
        IntPtr intPtr = lzip.zipDecompressBuffer(gchandle.AddrOfPinnedObject(), source.Length, ref num);
        if (num == 0 || intPtr == IntPtr.Zero)
        {
            gchandle.Free();
            lzip.releaseBuffer(intPtr);
            return false;
        }
        Array.Resize<byte>(ref outBuffer, num);
        Marshal.Copy(intPtr, outBuffer, 0, num);
        gchandle.Free();
        lzip.releaseBuffer(intPtr);
        return true;
    }

    // Token: 0x06002EB3 RID: 11955 RVA: 0x0010D268 File Offset: 0x0010B468
    public static int decompressBufferFixed(byte[] source, ref byte[] outBuffer, bool safe = true)
    {
        GCHandle gchandle = GCHandle.Alloc(source, GCHandleType.Pinned);
        int num = 0;
        IntPtr intPtr = lzip.zipDecompressBuffer(gchandle.AddrOfPinnedObject(), source.Length, ref num);
        if (num == 0 || intPtr == IntPtr.Zero)
        {
            gchandle.Free();
            lzip.releaseBuffer(intPtr);
            return 0;
        }
        if (num > outBuffer.Length)
        {
            if (safe)
            {
                gchandle.Free();
                lzip.releaseBuffer(intPtr);
                return 0;
            }
            num = outBuffer.Length;
        }
        Marshal.Copy(intPtr, outBuffer, 0, num);
        gchandle.Free();
        lzip.releaseBuffer(intPtr);
        return num;
    }

    // Token: 0x06002EB4 RID: 11956 RVA: 0x0010D2F4 File Offset: 0x0010B4F4
    public static byte[] decompressBuffer(byte[] source)
    {
        GCHandle gchandle = GCHandle.Alloc(source, GCHandleType.Pinned);
        int num = 0;
        IntPtr intPtr = lzip.zipDecompressBuffer(gchandle.AddrOfPinnedObject(), source.Length, ref num);
        if (num == 0 || intPtr == IntPtr.Zero)
        {
            gchandle.Free();
            lzip.releaseBuffer(intPtr);
            return null;
        }
        byte[] array = new byte[num];
        Marshal.Copy(intPtr, array, 0, num);
        gchandle.Free();
        lzip.releaseBuffer(intPtr);
        return array;
    }

    // Token: 0x06002EB5 RID: 11957 RVA: 0x0010D360 File Offset: 0x0010B560
    public static int entry2Buffer(string zipArchive, string entry, ref byte[] buffer, byte[] FileBuffer = null, string password = null)
    {
        if (password == string.Empty)
        {
            password = null;
        }
        int num;
        if (FileBuffer != null)
        {
            GCHandle gchandle = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            num = lzip.zipGetEntrySize(null, entry, gchandle.AddrOfPinnedObject(), FileBuffer.Length);
            gchandle.Free();
        }
        else
        {
            num = lzip.zipGetEntrySize(zipArchive, entry, IntPtr.Zero, 0);
        }
        if (num <= 0)
        {
            return -18;
        }
        Array.Resize<byte>(ref buffer, num);
        GCHandle gchandle2 = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        int result;
        if (FileBuffer != null)
        {
            GCHandle gchandle3 = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            result = lzip.zipEntry2Buffer(null, entry, gchandle2.AddrOfPinnedObject(), num, gchandle3.AddrOfPinnedObject(), FileBuffer.Length, password);
            gchandle3.Free();
        }
        else
        {
            result = lzip.zipEntry2Buffer(zipArchive, entry, gchandle2.AddrOfPinnedObject(), num, IntPtr.Zero, 0, password);
        }
        gchandle2.Free();
        return result;
    }

    // Token: 0x06002EB6 RID: 11958 RVA: 0x0010D42C File Offset: 0x0010B62C
    public static int entry2FixedBuffer(string zipArchive, string entry, ref byte[] fixedBuffer, byte[] FileBuffer = null, string password = null)
    {
        if (password == string.Empty)
        {
            password = null;
        }
        int num;
        if (FileBuffer != null)
        {
            GCHandle gchandle = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            num = lzip.zipGetEntrySize(null, entry, gchandle.AddrOfPinnedObject(), FileBuffer.Length);
            gchandle.Free();
        }
        else
        {
            num = lzip.zipGetEntrySize(zipArchive, entry, IntPtr.Zero, 0);
        }
        if (num <= 0)
        {
            return -18;
        }
        if (fixedBuffer.Length < num)
        {
            return -19;
        }
        GCHandle gchandle2 = GCHandle.Alloc(fixedBuffer, GCHandleType.Pinned);
        int num2;
        if (FileBuffer != null)
        {
            GCHandle gchandle3 = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            num2 = lzip.zipEntry2Buffer(null, entry, gchandle2.AddrOfPinnedObject(), num, gchandle3.AddrOfPinnedObject(), FileBuffer.Length, password);
            gchandle3.Free();
        }
        else
        {
            num2 = lzip.zipEntry2Buffer(zipArchive, entry, gchandle2.AddrOfPinnedObject(), num, IntPtr.Zero, 0, password);
        }
        gchandle2.Free();
        if (num2 != 1)
        {
            return num2;
        }
        return num;
    }

    // Token: 0x06002EB7 RID: 11959 RVA: 0x0010D508 File Offset: 0x0010B708
    public static byte[] entry2Buffer(string zipArchive, string entry, byte[] FileBuffer = null, string password = null)
    {
        if (password == string.Empty)
        {
            password = null;
        }
        int num;
        if (FileBuffer != null)
        {
            GCHandle gchandle = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            num = lzip.zipGetEntrySize(null, entry, gchandle.AddrOfPinnedObject(), FileBuffer.Length);
            gchandle.Free();
        }
        else
        {
            num = lzip.zipGetEntrySize(zipArchive, entry, IntPtr.Zero, 0);
        }
        if (num <= 0)
        {
            return null;
        }
        byte[] array = new byte[num];
        GCHandle gchandle2 = GCHandle.Alloc(array, GCHandleType.Pinned);
        int num2;
        if (FileBuffer != null)
        {
            GCHandle gchandle3 = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            num2 = lzip.zipEntry2Buffer(null, entry, gchandle2.AddrOfPinnedObject(), num, gchandle3.AddrOfPinnedObject(), FileBuffer.Length, password);
            gchandle3.Free();
        }
        else
        {
            num2 = lzip.zipEntry2Buffer(zipArchive, entry, gchandle2.AddrOfPinnedObject(), num, IntPtr.Zero, 0, password);
        }
        gchandle2.Free();
        if (num2 != 1)
        {
            return null;
        }
        return array;
    }

    // Token: 0x06002EB8 RID: 11960 RVA: 0x0010D5DC File Offset: 0x0010B7DC
    public static bool buffer2File(int levelOfCompression, string zipArchive, string arc_filename, byte[] buffer, bool append = false, string comment = null, string password = null, bool useBz2 = false)
    {
        if (!append && File.Exists(zipArchive))
        {
            File.Delete(zipArchive);
        }
        GCHandle gchandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        if (levelOfCompression < 0)
        {
            levelOfCompression = 0;
        }
        if (levelOfCompression > 9)
        {
            levelOfCompression = 9;
        }
        if (password == string.Empty)
        {
            password = null;
        }
        if (comment == string.Empty)
        {
            comment = null;
        }
        bool result = lzip.zipBuf2File(levelOfCompression, zipArchive, arc_filename, gchandle.AddrOfPinnedObject(), buffer.Length, comment, password, useBz2);
        gchandle.Free();
        return result;
    }

    // Token: 0x06002EB9 RID: 11961 RVA: 0x0010D668 File Offset: 0x0010B868
    public static int delete_entry(string zipArchive, string arc_filename)
    {
        string text = zipArchive + ".tmp";
        int num = lzip.zipDeleteFile(zipArchive, arc_filename, text);
        if (num > 0)
        {
            File.Delete(zipArchive);
            File.Move(text, zipArchive);
        }
        else if (File.Exists(text))
        {
            File.Delete(text);
        }
        return num;
    }

    // Token: 0x06002EBA RID: 11962 RVA: 0x0010D6B8 File Offset: 0x0010B8B8
    public static int replace_entry(string zipArchive, string arc_filename, string newFilePath, int level = 9, string comment = null, string password = null, bool useBz2 = false)
    {
        int num = lzip.delete_entry(zipArchive, arc_filename);
        if (num < 0)
        {
            return -3;
        }
        if (password == string.Empty)
        {
            password = null;
        }
        if (comment == string.Empty)
        {
            comment = null;
        }
        return lzip.zipCD(level, zipArchive, newFilePath, arc_filename, comment, password, useBz2);
    }

    // Token: 0x06002EBB RID: 11963 RVA: 0x0010D710 File Offset: 0x0010B910
    public static int replace_entry(string zipArchive, string arc_filename, byte[] newFileBuffer, int level = 9, string password = null, bool useBz2 = false)
    {
        int num = lzip.delete_entry(zipArchive, arc_filename);
        if (num < 0)
        {
            return -5;
        }
        if (lzip.buffer2File(level, zipArchive, arc_filename, newFileBuffer, true, null, password, useBz2))
        {
            return 1;
        }
        return -6;
    }

    // Token: 0x06002EBC RID: 11964 RVA: 0x0010D748 File Offset: 0x0010B948
    public static int extract_entry(string zipArchive, string arc_filename, string outpath, byte[] FileBuffer = null, int[] proc = null, string password = null)
    {
        if (!Directory.Exists(Path.GetDirectoryName(outpath)))
        {
            return -1;
        }
        if (proc == null)
        {
            proc = new int[1];
        }
        GCHandle gchandle = GCHandle.Alloc(proc, GCHandleType.Pinned);
        int result;
        if (FileBuffer != null)
        {
            GCHandle gchandle2 = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            if (proc != null)
            {
                result = lzip.zipEntry(null, arc_filename, outpath, gchandle2.AddrOfPinnedObject(), FileBuffer.Length, gchandle.AddrOfPinnedObject(), password);
            }
            else
            {
                result = lzip.zipEntry(null, arc_filename, outpath, gchandle2.AddrOfPinnedObject(), FileBuffer.Length, IntPtr.Zero, password);
            }
            gchandle2.Free();
            gchandle.Free();
            return result;
        }
        if (proc != null)
        {
            result = lzip.zipEntry(zipArchive, arc_filename, outpath, IntPtr.Zero, 0, gchandle.AddrOfPinnedObject(), password);
        }
        else
        {
            result = lzip.zipEntry(zipArchive, arc_filename, outpath, IntPtr.Zero, 0, IntPtr.Zero, password);
        }
        gchandle.Free();
        return result;
    }

    // Token: 0x06002EBD RID: 11965 RVA: 0x0010D824 File Offset: 0x0010BA24
    public static int decompress_File(string zipArchive, string outPath, int[] progress, byte[] FileBuffer = null, int[] proc = null, string password = null)
    {
        if (outPath.Substring(outPath.Length - 1, 1) != "/")
        {
            outPath += "/";
        }
        GCHandle gchandle = GCHandle.Alloc(progress, GCHandleType.Pinned);
        if (proc == null)
        {
            proc = new int[1];
        }
        GCHandle gchandle2 = GCHandle.Alloc(proc, GCHandleType.Pinned);
        int result;
        if (FileBuffer != null)
        {
            GCHandle gchandle3 = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            if (proc != null)
            {
                result = lzip.zipEX(null, outPath, gchandle.AddrOfPinnedObject(), gchandle3.AddrOfPinnedObject(), FileBuffer.Length, gchandle2.AddrOfPinnedObject(), password);
            }
            else
            {
                result = lzip.zipEX(null, outPath, gchandle.AddrOfPinnedObject(), gchandle3.AddrOfPinnedObject(), FileBuffer.Length, IntPtr.Zero, password);
            }
            gchandle3.Free();
            gchandle.Free();
            gchandle2.Free();
            return result;
        }
        if (proc != null)
        {
            result = lzip.zipEX(zipArchive, outPath, gchandle.AddrOfPinnedObject(), IntPtr.Zero, 0, gchandle2.AddrOfPinnedObject(), password);
        }
        else
        {
            result = lzip.zipEX(zipArchive, outPath, gchandle.AddrOfPinnedObject(), IntPtr.Zero, 0, IntPtr.Zero, password);
        }
        gchandle.Free();
        gchandle2.Free();
        return result;
    }

    // Token: 0x06002EBE RID: 11966 RVA: 0x0010D944 File Offset: 0x0010BB44
    public static int compress_File(int levelOfCompression, string zipArchive, string inFilePath, bool append = false, string fileName = "", string comment = null, string password = null, bool useBz2 = false)
    {
        if (!File.Exists(inFilePath))
        {
            return -10;
        }
        if (!append && File.Exists(zipArchive))
        {
            File.Delete(zipArchive);
        }
        if (fileName == string.Empty)
        {
            fileName = Path.GetFileName(inFilePath);
        }
        if (levelOfCompression < 0)
        {
            levelOfCompression = 0;
        }
        if (levelOfCompression > 9)
        {
            levelOfCompression = 9;
        }
        if (password == string.Empty)
        {
            password = null;
        }
        if (comment == string.Empty)
        {
            comment = null;
        }
        return lzip.zipCD(levelOfCompression, zipArchive, inFilePath, fileName, comment, password, useBz2);
    }

    // Token: 0x06002EBF RID: 11967 RVA: 0x0010D9E0 File Offset: 0x0010BBE0
    public static int compress_File_List(int levelOfCompression, string zipArchive, string[] inFilePath, int[] progress = null, bool append = false, string[] fileName = null, string password = null, bool useBz2 = false)
    {
        if (inFilePath == null)
        {
            return -3;
        }
        if (fileName != null && fileName.Length != inFilePath.Length)
        {
            return -4;
        }
        for (int i = 0; i < inFilePath.Length; i++)
        {
            if (!File.Exists(inFilePath[i]))
            {
                return -10;
            }
        }
        if (!append && File.Exists(zipArchive))
        {
            File.Delete(zipArchive);
        }
        if (levelOfCompression < 0)
        {
            levelOfCompression = 0;
        }
        if (levelOfCompression > 9)
        {
            levelOfCompression = 9;
        }
        if (password == string.Empty)
        {
            password = null;
        }
        int result = 0;
        string directoryName = Path.GetDirectoryName(zipArchive);
        string[] array;
        if (fileName == null)
        {
            array = new string[inFilePath.Length];
            for (int j = 0; j < inFilePath.Length; j++)
            {
                array[j] = inFilePath[j].Replace(directoryName, string.Empty);
            }
        }
        else
        {
            array = fileName;
        }
        for (int k = 0; k < inFilePath.Length; k++)
        {
            if (array[k] == null)
            {
                array[k] = inFilePath[k].Replace(directoryName, string.Empty);
            }
        }
        for (int l = 0; l < inFilePath.Length; l++)
        {
            if (progress != null)
            {
                progress[0]++;
            }
            result = lzip.compress_File(levelOfCompression, zipArchive, inFilePath[l], true, array[l], null, password, useBz2);
        }
        return result;
    }

    // Token: 0x06002EC0 RID: 11968 RVA: 0x0010DB38 File Offset: 0x0010BD38
    public static void compressDir(string sourceDir, int levelOfCompression, string zipArchive, bool includeRoot = false, string password = null, bool useBz2 = false)
    {
        string text = sourceDir.Replace("\\", "/");
        if (Directory.Exists(text))
        {
            if (File.Exists(zipArchive))
            {
                File.Delete(zipArchive);
            }
            string[] array = text.Split(new char[]
            {
                '/'
            });
            string text2 = array[array.Length - 1];
            string text3 = text2;
            lzip.cProgress = 0;
            if (levelOfCompression < 0)
            {
                levelOfCompression = 0;
            }
            if (levelOfCompression > 9)
            {
                levelOfCompression = 9;
            }
            foreach (string text4 in Directory.GetFiles(text, "*", SearchOption.AllDirectories))
            {
                string text5 = text4.Replace(text, text2).Replace("\\", "/");
                if (!includeRoot)
                {
                    text5 = text5.Substring(text3.Length + 1);
                }
                lzip.compress_File(levelOfCompression, zipArchive, text4, true, text5, null, password, useBz2);
                lzip.cProgress++;
            }
        }
    }

    // Token: 0x06002EC1 RID: 11969 RVA: 0x0010DC28 File Offset: 0x0010BE28
    public static int getAllFiles(string Dir)
    {
        string[] files = Directory.GetFiles(Dir, "*", SearchOption.AllDirectories);
        return files.Length;
    }

    // Token: 0x06002EC2 RID: 11970 RVA: 0x0010DC4C File Offset: 0x0010BE4C
    public static int gzip(byte[] source, byte[] outBuffer, int level, bool addHeader = true, bool addFooter = true)
    {
        GCHandle gchandle = GCHandle.Alloc(source, GCHandleType.Pinned);
        GCHandle gchandle2 = GCHandle.Alloc(outBuffer, GCHandleType.Pinned);
        int num = lzip.zipGzip(gchandle.AddrOfPinnedObject(), source.Length, gchandle2.AddrOfPinnedObject(), level, addHeader, addFooter);
        gchandle.Free();
        gchandle2.Free();
        int num2 = 0;
        if (addHeader)
        {
            num2 += 10;
        }
        if (addFooter)
        {
            num2 += 8;
        }
        return num + num2;
    }

    // Token: 0x06002EC3 RID: 11971 RVA: 0x0010DCB0 File Offset: 0x0010BEB0
    public static int gzipUncompressedSize(byte[] source)
    {
        int num = source.Length;
        return (int)(source[num - 4] & byte.MaxValue) | (int)(source[num - 3] & byte.MaxValue) << 8 | (int)(source[num - 2] & byte.MaxValue) << 16 | (int)(source[num - 1] & byte.MaxValue) << 24;
    }

    // Token: 0x06002EC4 RID: 11972 RVA: 0x0010DCFC File Offset: 0x0010BEFC
    public static int unGzip(byte[] source, byte[] outBuffer, bool hasHeader = true, bool hasFooter = true)
    {
        GCHandle gchandle = GCHandle.Alloc(source, GCHandleType.Pinned);
        GCHandle gchandle2 = GCHandle.Alloc(outBuffer, GCHandleType.Pinned);
        int result = lzip.zipUnGzip(gchandle.AddrOfPinnedObject(), source.Length, gchandle2.AddrOfPinnedObject(), outBuffer.Length, hasHeader, hasFooter);
        gchandle.Free();
        gchandle2.Free();
        return result;
    }

    // Token: 0x06002EC5 RID: 11973 RVA: 0x0010DD44 File Offset: 0x0010BF44
    public static int unGzip2(byte[] source, byte[] outBuffer)
    {
        GCHandle gchandle = GCHandle.Alloc(source, GCHandleType.Pinned);
        GCHandle gchandle2 = GCHandle.Alloc(outBuffer, GCHandleType.Pinned);
        int result = lzip.zipUnGzip2(gchandle.AddrOfPinnedObject(), source.Length, gchandle2.AddrOfPinnedObject(), outBuffer.Length);
        gchandle.Free();
        gchandle2.Free();
        return result;
    }

    // Token: 0x06002EC6 RID: 11974 RVA: 0x0010DD8C File Offset: 0x0010BF8C
    public static DateTime entryDateTime(string zipArchive, string entry, byte[] FileBuffer = null)
    {
        uint num = 0U;
        if (FileBuffer != null)
        {
            GCHandle gchandle = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            num = lzip.getEntryDateTime(null, entry, gchandle.AddrOfPinnedObject(), FileBuffer.Length);
            gchandle.Free();
        }
        if (FileBuffer == null)
        {
            num = lzip.getEntryDateTime(zipArchive, entry, IntPtr.Zero, 0);
        }
        uint num2 = (num & 4294901760U) >> 16;
        uint num3 = num & 65535U;
        uint year = (num2 >> 9) + 1980U;
        uint month = (num2 & 480U) >> 5;
        uint day = num2 & 31U;
        uint hour = num3 >> 11;
        uint minute = (num3 & 2016U) >> 5;
        uint second = (num3 & 31U) * 2U;
        if (num == 0U || num == 1U || num == 2U)
        {
            Debug.Log("Error in getting DateTime");
            return DateTime.Now;
        }
        return new DateTime((int)year, (int)month, (int)day, (int)hour, (int)minute, (int)second);
    }

    // Token: 0x04001DD8 RID: 7640
    private const string libname = "zipw";

    // Token: 0x04001DD9 RID: 7641
    public static List<string> ninfo = new List<string>();

    // Token: 0x04001DDA RID: 7642
    public static List<long> uinfo = new List<long>();

    // Token: 0x04001DDB RID: 7643
    public static List<long> cinfo = new List<long>();

    // Token: 0x04001DDC RID: 7644
    public static int zipFiles;

    // Token: 0x04001DDD RID: 7645
    public static int zipFolders;

    // Token: 0x04001DDE RID: 7646
    public static int cProgress = 0;

    // Token: 0x0200055B RID: 1371
    public struct fileStat
    {
        // Token: 0x04001DDF RID: 7647
        public int index;

        // Token: 0x04001DE0 RID: 7648
        public int compSize;

        // Token: 0x04001DE1 RID: 7649
        public int uncompSize;

        // Token: 0x04001DE2 RID: 7650
        public int nameSize;

        // Token: 0x04001DE3 RID: 7651
        public string name;

        // Token: 0x04001DE4 RID: 7652
        public int commentSize;

        // Token: 0x04001DE5 RID: 7653
        public string comment;

        // Token: 0x04001DE6 RID: 7654
        public bool isDirectory;

        // Token: 0x04001DE7 RID: 7655
        public bool isSupported;

        // Token: 0x04001DE8 RID: 7656
        public bool isEncrypted;
    }
}
