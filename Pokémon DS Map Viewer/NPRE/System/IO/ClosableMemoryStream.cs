namespace System.IO
{
    using System;

    public class ClosableMemoryStream : MemoryStream
    {
        private bool _closeable = true;

        public override void Close()
        {
            if (  _closeable)
            {
                base.Close();
            }
        }

        public void ForceClose()
        {
            base.Close();
        }

        public bool Closeable
        {
            get
            {
                return   _closeable;
            }
            set
            {
                  _closeable = value;
            }
        }
    }
}

