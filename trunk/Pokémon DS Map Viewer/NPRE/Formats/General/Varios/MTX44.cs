namespace PG4Map.Formats
{
    using System;
    using System.Reflection;

    public class MTX44
    {
        public float[] _array = new float[0x10];

        public MTX44 Clone()
        {
            MTX44 mtx = new MTX44();
            for (int i = 0; i < 0x10; i++)
            {
                mtx._array[i] =   _array[i];
            }
            return mtx;
        }

        public void CopyValuesTo(MTX44 m)
        {
            for (int i = 0; i < 0x10; i++)
            {
                m._array[i] = this[i];
            }
        }

        public void LoadIdentity()
        {
            float num;
              Zero();
            this[3, 3] = num = 1f;
            this[2, 2] = num = num;
            this[0, 0] = this[1, 1] = num;
        }

        public MTX44 MultMatrix(MTX44 b)
        {
            MTX44 mtx = new MTX44();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    mtx[(i << 2) + j] = 0f;
                    for (int k = 0; k < 4; k++)
                    {
                        MTX44 mtx4;
                        int num4;
                        (mtx4 = mtx)[num4 = (i << 2) + j] = mtx4[num4] + (mtx[(k << 2) + j] * b[(i << 2) + k]);
                    }
                }
            }
            return mtx;
        }

        public float[] MultVector(float[] v)
        {
            MTX44 mtx = this;
            float[] numArray = new float[3];
            float num = v[0];
            float num2 = v[1];
            float num3 = v[2];
            numArray[0] = (((num * mtx[0]) + (num2 * mtx[4])) + (num3 * mtx[8])) + mtx[12];
            numArray[1] = (((num * mtx[1]) + (num2 * mtx[5])) + (num3 * mtx[9])) + mtx[13];
            numArray[2] = (((num * mtx[2]) + (num2 * mtx[6])) + (num3 * mtx[10])) + mtx[14];
            return numArray;
        }

        public void Scale(float x, float y, float z)
        {
            MTX44 b = new MTX44();
            b.LoadIdentity();
            b[0] = x;
            b[5] = y;
            b[10] = z;
              MultMatrix(b).CopyValuesTo(this);
        }

        public void Zero()
        {
            for (int i = 0; i < 0x10; i++)
            {
                  _array[i] = 0f;
            }
        }

        public float[] Floats
        {
            get
            {
                return   _array;
            }
        }

        public float this[int x, int y]
        {
            get
            {
                return   _array[x + (y * 4)];
            }
            set
            {
                  _array[x + (y * 4)] = value;
            }
        }

        public float this[int index]
        {
            get
            {
                return   _array[index];
            }
            set
            {
                  _array[index] = value;
            }
        }
    }
}

