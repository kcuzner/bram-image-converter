using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace ImageConverter
{
	public class ImageCollection : ICollection<Bitmap>, IDisposable
    {
		private IList<Bitmap> _bitmaps;

        public ImageCollection()
        {
			this._bitmaps = new List<Bitmap>();
        }

		/// <summary>
		/// Creates a new image from this collection
		/// </summary>
		public Bitmap Render()
		{
			int width = this._bitmaps.Max(b => b.Width);
			int height = this._bitmaps.Sum(b => b.Height);

			Bitmap bitmap = new Bitmap(width, height);

			using(Graphics g = Graphics.FromImage(bitmap))
			{
				int cy = 0; //current y position
				for(int i = 0; i < this._bitmaps.Count; i++)
				{
					Bitmap b = this._bitmaps[i];
					Point p = new Point(0, cy);
					g.DrawImage(b, p);

					cy += b.Height;
				}
			}

			return bitmap;
		}

		#region ICollection implementation

		public void Add(Bitmap item)
		{
			this._bitmaps.Add(item);
		}

		public void Clear()
		{
			this._bitmaps.Clear();
		}

		public bool Contains(Bitmap item)
		{
			return this._bitmaps.Contains(item);
		}

		public void CopyTo(Bitmap[] array, int arrayIndex)
		{
			this._bitmaps.CopyTo(array, arrayIndex);
		}

		public bool Remove(Bitmap item)
		{
			return this._bitmaps.Remove(item);
		}

		public int Count
		{
			get
			{
				return this._bitmaps.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return this._bitmaps.IsReadOnly;
			}
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<Bitmap> GetEnumerator()
		{
			return this._bitmaps.GetEnumerator();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this._bitmaps.GetEnumerator();
		}

		#endregion

		#region IDisposable implementation

		public void Dispose()
		{
			foreach (var b in this._bitmaps)
				b.Dispose();
		}

		#endregion
    }
}

