//namespace DSDecmp.Formats.Nitro
//{
//    using DSDecmp.Utils;
//    using System;
//    using System.Collections.Generic;
//    using System.IO;
//    using System.Runtime.CompilerServices;
//    using System.Runtime.InteropServices;

//    public class Huffman : NitroCFormat
//    {
//        static Huffman()
//        {
//            CompressBlockSize = BlockSize.EIGHTBIT;
//        }

//        public Huffman() : base(0)
//        {
//        }

//        public override int Compress(Stream instream, long inLength, Stream outstream)
//        {
//            switch (CompressBlockSize)
//            {
//                case BlockSize.FOURBIT:
//                    return   Compress4(instream, inLength, outstream);

//                case BlockSize.EIGHTBIT:
//                    return   Compress8(instream, inLength, outstream);
//            }
//            return 0;
//        }

//        private int Compress4(Stream instream, long inLength, Stream outstream)
//        {
//            int num;
//            HuffTreeNode parent;
//            int num5;
//            byte num7;
//            byte[] buffer = new byte[inLength];
//            instream.Read(buffer, 0, (int) inLength);
//            int[] numArray = new int[0x10];
//            for (num = 0; num < inLength; num++)
//            {
//                numArray[buffer[num] & 15]++;
//                numArray[(buffer[num] >> 4) & 15]++;
//            }
//            SimpleReversedPrioQueue<int, HuffTreeNode> leafQueue = new SimpleReversedPrioQueue<int, HuffTreeNode>();
//            SimpleReversedPrioQueue<int, HuffTreeNode> nodeQueue = new SimpleReversedPrioQueue<int, HuffTreeNode>();
//            int num2 = 0;
//            HuffTreeNode[] nodeArray = new HuffTreeNode[0x10];
//            for (num = 0; num < 0x10; num++)
//            {
//                if (numArray[num] != 0)
//                {
//                    parent = new HuffTreeNode((byte) num, true, null, null);
//                    nodeArray[num] = parent;
//                    leafQueue.Enqueue(numArray[num], parent);
//                    num2++;
//                }
//            }
//            while ((leafQueue.Count + nodeQueue.Count) > 1)
//            {
//                int num3;
//                int num4;
//                HuffTreeNode node2 = null;
//                HuffTreeNode node3 = null;
//                node2 =   GetLowest(leafQueue, nodeQueue, out num3);
//                node3 =   GetLowest(leafQueue, nodeQueue, out num4);
//                HuffTreeNode node4 = new HuffTreeNode(0, false, node2, node3);
//                nodeQueue.Enqueue(num3 + num4, node4);
//                num2++;
//            }
//            HuffTreeNode node5 = nodeQueue.Dequeue(out num5);
//            node5.Depth = 0;
//            outstream.WriteByte(0x24);
//            outstream.WriteByte((byte) (inLength & 0xffL));
//            outstream.WriteByte((byte) ((inLength >> 8) & 0xffL));
//            outstream.WriteByte((byte) ((inLength >> 0x10) & 0xffL));
//            int num6 = 4;
//            outstream.WriteByte((byte) ((num2 - 1) / 2));
//            num6++;
//            LinkedList<HuffTreeNode> list = new LinkedList<HuffTreeNode>();
//            list.AddLast(node5);
//            while (list.Count > 0)
//            {
//                parent = list.First.Value;
//                list.RemoveFirst();
//                if (parent.IsData)
//                {
//                    outstream.WriteByte(parent.Data);
//                }
//                else
//                {
//                    num7 = (byte) (list.Count / 2);
//                    num7 = (byte) (num7 & 0x3f);
//                    if (parent.Child0.IsData)
//                    {
//                        num7 = (byte) (num7 | 0x80);
//                    }
//                    if (parent.Child1.IsData)
//                    {
//                        num7 = (byte) (num7 | 0x40);
//                    }
//                    outstream.WriteByte(num7);
//                    list.AddLast(parent.Child0);
//                    list.AddLast(parent.Child1);
//                }
//                num6++;
//            }
//            uint num8 = 0;
//            byte num9 = 0x20;
//            for (num = 0; num < inLength; num++)
//            {
//                num7 = buffer[num];
//                for (int i = 0; i < 2; i++)
//                {
//                    parent = nodeArray[(num7 >> (4 - (i * 4))) & 15];
//                    int depth = parent.Depth;
//                    bool[] flagArray = new bool[depth];
//                    int index = 0;
//                    while (index < depth)
//                    {
//                        flagArray[(depth - index) - 1] = parent.IsChild1;
//                        parent = parent.Parent;
//                        index++;
//                    }
//                    for (index = 0; index < depth; index++)
//                    {
//                        if (num9 == 0)
//                        {
//                            outstream.Write(IOUtils.FromNDSu32(num8), 0, 4);
//                            num6 += 4;
//                            num8 = 0;
//                            num9 = 0x20;
//                        }
//                        num9 = (byte) (num9 - 1);
//                        if (flagArray[index])
//                        {
//                            num8 |= ((uint) 1) << num9;
//                        }
//                    }
//                }
//            }
//            if (num9 != 0x20)
//            {
//                outstream.Write(IOUtils.FromNDSu32(num8), 0, 4);
//                num6 += 4;
//            }
//            return num6;
//        }

//        private int Compress8(Stream instream, long inLength, Stream outstream)
//        {
//            int num;
//            HuffTreeNode parent;
//            int num5;
//            byte num7;
//            byte[] buffer = new byte[inLength];
//            instream.Read(buffer, 0, (int) inLength);
//            int[] numArray = new int[0x100];
//            for (num = 0; num < inLength; num++)
//            {
//                numArray[buffer[num]]++;
//            }
//            SimpleReversedPrioQueue<int, HuffTreeNode> leafQueue = new SimpleReversedPrioQueue<int, HuffTreeNode>();
//            SimpleReversedPrioQueue<int, HuffTreeNode> nodeQueue = new SimpleReversedPrioQueue<int, HuffTreeNode>();
//            int num2 = 0;
//            HuffTreeNode[] nodeArray = new HuffTreeNode[0x100];
//            for (num = 0; num < 0x100; num++)
//            {
//                if (numArray[num] != 0)
//                {
//                    parent = new HuffTreeNode((byte) num, true, null, null);
//                    nodeArray[num] = parent;
//                    leafQueue.Enqueue(numArray[num], parent);
//                    num2++;
//                }
//            }
//            while ((leafQueue.Count + nodeQueue.Count) > 1)
//            {
//                int num3;
//                int num4;
//                HuffTreeNode node2 = null;
//                HuffTreeNode node3 = null;
//                node2 =   GetLowest(leafQueue, nodeQueue, out num3);
//                node3 =   GetLowest(leafQueue, nodeQueue, out num4);
//                HuffTreeNode node4 = new HuffTreeNode(0, false, node2, node3);
//                nodeQueue.Enqueue(num3 + num4, node4);
//                num2++;
//            }
//            HuffTreeNode node5 = nodeQueue.Dequeue(out num5);
//            node5.Depth = 0;
//            outstream.WriteByte(40);
//            outstream.WriteByte((byte) (inLength & 0xffL));
//            outstream.WriteByte((byte) ((inLength >> 8) & 0xffL));
//            outstream.WriteByte((byte) ((inLength >> 0x10) & 0xffL));
//            int num6 = 4;
//            outstream.WriteByte((byte) ((num2 - 1) / 2));
//            num6++;
//            LinkedList<HuffTreeNode> list = new LinkedList<HuffTreeNode>();
//            list.AddLast(node5);
//            while (list.Count > 0)
//            {
//                parent = list.First.Value;
//                list.RemoveFirst();
//                if (parent.IsData)
//                {
//                    outstream.WriteByte(parent.Data);
//                }
//                else
//                {
//                    num7 = (byte) (list.Count / 2);
//                    num7 = (byte) (num7 & 0x3f);
//                    if (parent.Child0.IsData)
//                    {
//                        num7 = (byte) (num7 | 0x80);
//                    }
//                    if (parent.Child1.IsData)
//                    {
//                        num7 = (byte) (num7 | 0x40);
//                    }
//                    outstream.WriteByte(num7);
//                    list.AddLast(parent.Child0);
//                    list.AddLast(parent.Child1);
//                }
//                num6++;
//            }
//            uint num8 = 0;
//            byte num9 = 0x20;
//            for (num = 0; num < inLength; num++)
//            {
//                num7 = buffer[num];
//                parent = nodeArray[num7];
//                int depth = parent.Depth;
//                bool[] flagArray = new bool[depth];
//                int index = 0;
//                while (index < depth)
//                {
//                    flagArray[(depth - index) - 1] = parent.IsChild1;
//                    parent = parent.Parent;
//                    index++;
//                }
//                for (index = 0; index < depth; index++)
//                {
//                    if (num9 == 0)
//                    {
//                        outstream.Write(IOUtils.FromNDSu32(num8), 0, 4);
//                        num6 += 4;
//                        num8 = 0;
//                        num9 = 0x20;
//                    }
//                    num9 = (byte) (num9 - 1);
//                    if (flagArray[index])
//                    {
//                        num8 |= ((uint) 1) << num9;
//                    }
//                }
//            }
//            if (num9 != 0x20)
//            {
//                outstream.Write(IOUtils.FromNDSu32(num8), 0, 4);
//                num6 += 4;
//            }
//            return num6;
//        }

//        public long Decompress(Stream instream, long inLength, Stream outstream)
//        {
//            long num = 0L;
//            byte num2 = (byte) instream.ReadByte();
//            BlockSize fOURBIT = BlockSize.FOURBIT;
//            if (num2 != fOURBIT)
//            {
//                fOURBIT = BlockSize.EIGHTBIT;
//            }
//            byte[] buffer = new byte[3];
//            instream.Read(buffer, 0, 3);
//            int num3 = IOUtils.ToNDSu24(buffer, 0);
//            num += 4L;
//            if (num3 == 0)
//            {
//                buffer = new byte[4];
//                instream.Read(buffer, 0, 4);
//                num3 = IOUtils.ToNDSs32(buffer, 0);
//                num += 4L;
//            }
//            int num4 = instream.ReadByte();
//            num += 1L;
//            num4 = (num4 + 1) * 2;
//            long maxStreamPos = (instream.Position - 1L) + num4;
//            HuffTreeNode node = new HuffTreeNode(instream, false, 5L, maxStreamPos);
//            num += num4;
//            instream.Position = maxStreamPos;
//            uint num6 = 0;
//            byte num7 = 0;
//            int num8 = -1;
//            int num9 = 0;
//            HuffTreeNode node2 = node;
//            byte[] buffer2 = new byte[4];
//            while (num9 < num3)
//            {
//                while (!node2.IsData)
//                {
//                    if (num7 == 0)
//                    {
//                        int num10 = instream.Read(buffer2, 0, 4);
//                        num += num10;
//                        num6 = IOUtils.ToNDSu32(buffer2, 0);
//                        num7 = 0x20;
//                    }
//                    num7 = (byte) (num7 - 1);
//                    node2 = ((num6 & (((int) 1) << num7)) != 0L) ? node2.Child1 : node2.Child0;
//                }
//                switch (fOURBIT)
//                {
//                    case BlockSize.FOURBIT:
//                        if (num8 < 0)
//                        {
//                            num8 = node2.Data << 4;
//                        }
//                        else
//                        {
//                            num8 |= node2.Data;
//                            outstream.WriteByte((byte) num8);
//                            num9++;
//                            num8 = -1;
//                        }
//                        break;

//                    case BlockSize.EIGHTBIT:
//                        outstream.WriteByte(node2.Data);
//                        num9++;
//                        break;
//                }
//                outstream.Flush();
//                node2 = node;
//            }
//            if ((num % 4L) != 0L)
//            {
//                num += 4L - (num % 4L);
//            }
//            return (long) num3;
//        }

//        private HuffTreeNode GetLowest(SimpleReversedPrioQueue<int, HuffTreeNode> leafQueue, SimpleReversedPrioQueue<int, HuffTreeNode> nodeQueue, out int prio)
//        {
//            if (leafQueue.Count != 0)
//            {
//                int num;
//                int num2;
//                if (nodeQueue.Count == 0)
//                {
//                    return leafQueue.Dequeue(out prio);
//                }
//                leafQueue.Peek(out num);
//                nodeQueue.Peek(out num2);
//                if (num <= num2)
//                {
//                    return leafQueue.Dequeue(out prio);
//                }
//            }
//            return nodeQueue.Dequeue(out prio);
//        }

//        public override bool Supports(Stream stream, long inLength)
//        {
//            base.magicByte = 0x24;
//            if (base.Supports(stream, inLength))
//            {
//                return true;
//            }
//            base.magicByte = 40;
//            return base.Supports(stream, inLength);
//        }

//        //public static BlockSize CompressBlockSize
//        //{
//        //    [CompilerGenerated]
//        //    get
//        //    {
//        //        return <CompressBlockSize>k__BackingField;
//        //    }
//        //    [CompilerGenerated]
//        //    set
//        //    {
//        //        <CompressBlockSize>k__BackingField = value;
//        //    }
//        //}

//        public enum BlockSize : byte
//        {
//            EIGHTBIT = 40,
//            FOURBIT = 0x24
//        }

//        public class HuffTreeNode
//        {
//            private Huffman.HuffTreeNode child0;
//            private Huffman.HuffTreeNode child1;
//            private byte data;
//            private int depth;
//            private bool isData;
//            private bool isFilled;

//            public HuffTreeNode(byte data, bool isData, Huffman.HuffTreeNode child0, Huffman.HuffTreeNode child1)
//            {
//                  data = data;
//                  isData = isData;
//                  child0 = child0;
//                  child1 = child1;
//                  isFilled = true;
//                if (!isData)
//                {
//                      child0.Parent = this;
//                      child1.Parent = this;
//                }
//            }

//            public HuffTreeNode(Stream stream, bool isData, long relOffset, long maxStreamPos)
//            {
//                if (stream.Position >= maxStreamPos)
//                {
//                      isFilled = false;
//                }
//                else
//                {
//                      isFilled = true;
//                    int num = stream.ReadByte();
//                      data = (byte) num;
//                      isData = isData;
//                    if (!  isData)
//                    {
//                        int num2 =   data & 0x3f;
//                        bool flag = (  data & 0x80) > 0;
//                        bool flag2 = (  data & 0x40) > 0;
//                        long num3 = ((relOffset ^ (relOffset & 1L)) + (num2 * 2)) + 2L;
//                        long position = stream.Position;
//                        stream.Position += (num3 - relOffset) - 1L;
//                          child0 = new Huffman.HuffTreeNode(stream, flag, num3, maxStreamPos);
//                          child0.Parent = this;
//                          child1 = new Huffman.HuffTreeNode(stream, flag2, num3 + 1L, maxStreamPos);
//                          child1.Parent = this;
//                        stream.Position = position;
//                    }
//                }
//            }

//            public override string ToString()
//            {
//                if (  isData)
//                {
//                    return ("<" +   data.ToString("X2") + ">");
//                }
//                return ("[" +   child0.ToString() + "," +   child1.ToString() + "]");
//            }

//            public Huffman.HuffTreeNode Child0
//            {
//                get
//                {
//                    return   child0;
//                }
//            }

//            public Huffman.HuffTreeNode Child1
//            {
//                get
//                {
//                    return   child1;
//                }
//            }

//            public byte Data
//            {
//                get
//                {
//                    return   data;
//                }
//            }

//            public int Depth
//            {
//                get
//                {
//                    return   depth;
//                }
//                set
//                {
//                      depth = value;
//                    if (!  isData)
//                    {
//                          child0.Depth =   depth + 1;
//                          child1.Depth =   depth + 1;
//                    }
//                }
//            }

//            public bool IsChild0
//            {
//                get
//                {
//                    return (  Parent.child0 == this);
//                }
//            }

//            public bool IsChild1
//            {
//                get
//                {
//                    return (  Parent.child1 == this);
//                }
//            }

//            public bool IsData
//            {
//                get
//                {
//                    return   isData;
//                }
//            }

//            public Huffman.HuffTreeNode Parent { get; private set; }
//        }
//    }
//}

