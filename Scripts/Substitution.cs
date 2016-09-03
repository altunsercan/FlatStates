namespace ninja.marching.flatstates
{
    public struct Substitution
    {
        public Term original;
        public Term substituted;

        public static bool operator ==(Substitution x, Substitution y)
        {
            return x.original == y.original && x.substituted == y.substituted;
        }

        public static bool operator !=(Substitution x, Substitution y)
        {
            return !(x == y);
        }

        public override string ToString ()
        {
            return string.Format ("{0}/{1}", original.ToString(), substituted.ToString());
        }
    }
}