using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace ImageConverter
{
	public class ImageCollection : ICollection<Image>, IDisposable
    {
		private IList<Image> _images;

        public ImageCollection()
        {
			this._images = new List<Image>();
        }

		/// <summary>
		/// Creates a new image from this collection
		/// </summary>
		public Bitmap Render()
		{
			if (this._images.Count == 0)
				throw new InvalidOperationException("At least one image must be loaded");

			int width = this._images.Max(b => b.Width);
			int height = this._images.Sum(b => b.Height);

			Bitmap bitmap = new Bitmap(width, height);

			using(Graphics g = Graphics.FromImage(bitmap))
			{
				int cy = 0; //current y position
				for(int i = 0; i < this._images.Count; i++)
				{
					Image img = this._images[i];
					Point p = new Point(0, cy);
					g.DrawImage(img, p);

					cy += img.Height;
				}
			}

			return bitmap;
		}

		#region ICollection implementation

		public void Add(Image item)
		{
			this._images.Add(item);
		}

		public void Clear()
		{
			this._images.Clear();
		}

		public bool Contains(Image item)
		{
			return this._images.Contains(item);
		}

		public void CopyTo(Image[] array, int arrayIndex)
		{
			this._images.CopyTo(array, arrayIndex);
		}

		public bool Remove(Image item)
		{
			return this._images.Remove(item);
		}

		public int Count
		{
			get
			{
				return this._images.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return this._images.IsReadOnly;
			}
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<Image> GetEnumerator()
		{
			return this._images.GetEnumerator();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this._images.GetEnumerator();
		}

		#endregion

		#region IDisposable implementation

		public void Dispose()
		{
			foreach (var i in this._images)
				i.Dispose();
		}

		#endregion
    }
}

