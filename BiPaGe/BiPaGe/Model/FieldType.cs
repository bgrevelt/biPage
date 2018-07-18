namespace BiPaGe.Model
{
    public abstract class FieldType
    {
        public abstract bool HasStaticSize();
        // TODO: not really happy with this. In practice this comes down to SizeInBits throwing is HasStaticSize is false; That seeems like bad design..
        // For now we just build something that works, but we definately need to improve on this in future iterations.
        public abstract uint SizeInBits();
    }
}
