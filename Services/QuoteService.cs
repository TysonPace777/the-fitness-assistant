namespace the_fitness_assistant.Services;

public class QuoteService
{
    private readonly List<string> _quotes =
    [
        "Small choices repeated daily create extraordinary results.",
        "Your body is built by the habits you practice, not the goals you wish for.",
        "Progress is progress, whether the step is big or small.",
        "Take care of your body today; it is the home you will live in for the rest of your life.",
        "You do not have to be perfect. You only have to keep moving forward.",
        "The strongest version of yourself is built one healthy choice at a time.",
        "Discipline is choosing what you want most over what you want right now.",
        "A healthy lifestyle is not a temporary challenge; it is a lifelong investment.",
        "Every workout is a vote for the person you want to become.",
        "Nourish your body with the same kindness you would offer someone you love.",
        "Consistency beats intensity when building habits that last.",
        "Your future self will thank you for the choices you make today.",
        "The journey may be slow, but every step takes you closer to your goal.",
        "Healthy eating is not about restriction; it is about giving your body what it needs.",
        "Strength is built through patience, persistence, and practice.",
        "You are not starting over; you are continuing your journey with more experience.",
        "A single healthy meal will not transform your life, but the habit of healthy meals will.",
        "Celebrate your progress, not just your destination.",
        "The best workout is the one you can consistently return to.",
        "Your health is one of the greatest gifts you can give yourself and those you love.",
        "Small improvements every day add up to remarkable change.",
        "Listen to your body, respect your limits, and keep growing.",
        "The hardest step is often the first one. Take it anyway.",
        "Healthy habits create a foundation for a stronger, happier life.",
        "You are capable of more than you realize when you commit to yourself.",
        "Fuel your body. Strengthen your mind. Believe in your ability to change.",
        "Every day is another opportunity to make a choice that supports your goals.",
        "Do not compare your journey to someone else's. Focus on becoming your best self.",
        "Motivation gets you started; habits keep you moving.",
        "The goal is not just to lose weight or gain strength. The goal is to build a healthier life."
    ];

    private readonly Random _random = new();

    public string GetRandomQuote()
    {
        int index = _random.Next(_quotes.Count);

        return _quotes[index];
    }
}