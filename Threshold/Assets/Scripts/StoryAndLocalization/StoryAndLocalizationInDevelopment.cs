using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class StoryAndLocalizationInDevelopment : MonoBehaviour
{

    #region StoryScript
public Dictionary<int, Dictionary<string, string>> ScriptTranslations = new Dictionary<int, Dictionary<string, string>>()
{
    {0, new Dictionary<string, string>()
        {
            {"ko", "6시에 일어나서, 회의를 퇴근 시간까지 하고, 퇴근 시간부터 밤 11시 까지 일하고."},
            {"en", "Wake up at 6, meetings until quitting time, work from quitting time until 11 PM."},
            {"ja", "6時に起きて、退社時間まで会議をして、退社時間から夜11時まで働く。"}
        }
    },
    {1, new Dictionary<string, string>()
        {
            {"ko", "다시 1시에 자서. 6시 일어나고. 이거 들었을 때 거부감."},
            {"en", "Sleep at 1 AM, wake up at 6 again. Feeling averse to this."},
            {"ja", "また1時に寝て、6時に起きる。これを聞いたときの拒否感。"}
        }
    },
    {2, new Dictionary<string, string>()
        {
            {"ko", "아무리 일을 즐겨도 이렇게 하면 힘들지 않을까 싶은데."},
            {"en", "Even if you enjoy work, wouldn't this be tough?"},
            {"ja", "どんなに仕事が好きでも、こんなことをしたら辛くないだろうか。"}
        }
    },
    {3, new Dictionary<string, string>()
        {
            {"ko", "힘들겠다고 생각한 시점에서, 내가 일을 좋아하지 않는 건가? 거짓인가? 싶은 생각."},
            {"en", "When I think it's tough, I wonder if I don't actually like work? Is it a lie?"},
            {"ja", "辛いと思った時点で、私は仕事が好きじゃないのかな？嘘なのかな？という考え。"}
        }
    },
    {4, new Dictionary<string, string>()
        {
            {"ko", "어릴 때 일과도 관련이 있겠다만, 즐거우려고 사는 게 아닌가… 솔직히 공감이 잘 안 된다…"},
            {"en", "It might be related to childhood work, but aren't we living to be happy? Honestly, I can't really empathize..."},
            {"ja", "子供の頃の仕事とも関係があるだろうけど、楽しむために生きているんじゃないか…正直、共感できない…"}
        }
    },
    {5, new Dictionary<string, string>()
        {
            {"ko", "남을 깍아내리지 말기. 인간관계에 대한 부러움을 갖는다면, 그걸 조금 더 추구하면 된다."},
            {"en", "Don't put others down. If you envy someone's relationships, pursue that a bit more."},
            {"ja", "他人を貶めないこと。人間関係に憧れを持つなら、それをもう少し追求すればいい。"}
        }
    },
    {6, new Dictionary<string, string>()
        {
            {"ko", "부러움 혹은 동경이 있다면, 그것과 가까워질 수 있는 길로 더 나아갈 수 있도록. 그런데 딱히 매몰되지는 않았으면…"},
            {"en", "If you have envy or admiration, move towards getting closer to that. But don't get too caught up in it..."},
            {"ja", "憧れや羨望があるなら、それに近づける道をさらに進めるように。でも、特にのめり込まないように…"}
        }
    },
    {7, new Dictionary<string, string>()
        {
            {"ko", "다른 사람에 대해, 아무리 그걸 나쁘게 했다고 한들, 뒤에서 뭐라 하는 건 좀 별로가 아닐까."},
            {"en", "No matter how badly someone did something, isn't it not good to talk behind their back?"},
            {"ja", "他人について、どんなに悪いことをしたとしても、陰で何か言うのはよくないんじゃないか。"}
        }
    },
    {8, new Dictionary<string, string>()
        {
            {"ko", "으음... 하지 않을 수 있도록 노력해야겠다. 그런데 짜증.... 듣는 것도 달리 듣고. 딴지도 계속 걸고."},
            {"en", "Hmm... I should try not to do that. But the irritation... Hearing things differently. Constantly picking faults."},
            {"ja", "うーん...しないように努力しなければ。でもイライラ...。聞くことも違って聞こえて。ちょっかいも出し続けて。"}
        }
    },
    {9, new Dictionary<string, string>()
        {
            {"ko", "하… 받기 힘듬. 다 잠이 부족해서 그런가. 최근에 잠이 부족하다…"},
            {"en", "Ugh... it's hard to take. Is it all because of lack of sleep? I've been lacking sleep lately..."},
            {"ja", "はぁ...耐えるのが難しい。全部睡眠不足のせいかな。最近睡眠不足だ..."}
        }
    },
    {10, new Dictionary<string, string>()
        {
            {"ko", "내가 그렇게 잘난 것도 아닌데, 계속해서 짜증내고. 좀 더 되돌아봐야 하지 않을까…"},
            {"en", "I'm not that great myself, but I keep getting irritated. Shouldn't I reflect more?"},
            {"ja", "私はそんなに偉くもないのに、イライラし続けて。もう少し振り返るべきじゃないか..."}
        }
    },
    {11, new Dictionary<string, string>()
        {
            {"ko", "왜 이런 것들로 고민을해야하는 건지 모르겠네. 왜 이렇게 잘난 사람들이 많을까?"},
            {"en", "I don't know why I have to worry about these things. Why are there so many impressive people?"},
            {"ja", "なぜこんなことで悩まなければならないのかわからない。なぜこんなに優秀な人が多いのだろうか？"}
        }
    },
    {12, new Dictionary<string, string>()
        {
            {"ko", "이런 사람들은 실제에서는 10%가 될까 말까 일텐데. 주변도 보면… 잘 모르겠는 사람들이..."},
            {"en", "In reality, these people might barely make up 10%. Looking around... people I'm not sure about..."},
            {"ja", "実際にはこういう人たちは10％もいないだろう。周りを見ても...よくわからない人たち..."}
        }
    },
    {13, new Dictionary<string, string>()
        {
            {"ko", "몰두하거나 몰입하는 시간이 달라 생기는 문제이지 않을까 싶긴 한데.. 그걸 지금 내가 보는 입장이니까, 솔직히 잘 모르겠다."},
            {"en", "I think it might be a problem of different times spent focusing or immersing, but since I'm the one seeing it now, honestly, I'm not sure."},
            {"ja", "集中したり没頭する時間が違うことから生じる問題なのかもしれないけど...それを今の私が見ている立場だから、正直よくわからない。"}
        }
    },
    {14, new Dictionary<string, string>()
        {
            {"ko", "자기 잘난 듯이 사는 사람은 같이 일하기 힘들 것. 덜 예민하기."},
            {"en", "People who live like they're all that must be hard to work with. Be less sensitive."},
            {"ja", "自分が偉いように生きている人は一緒に仕事をするのが大変だろう。もっと鈍感になること。"}
        }
    },
    {15, new Dictionary<string, string>()
        {
            {"ko", "근래 좀 많이 예민해진 것 같은데, 스스로를 챙길 수 있도록 하자."},
            {"en", "I seem to have become quite sensitive lately, let's take care of ourselves."},
            {"ja", "最近少し敏感になっているようだが、自分自身を大切にしよう。"}
        }
    },
    {16, new Dictionary<string, string>()
        {
            {"ko", "두루 많이 친하게 지내는 사람들을 보면 신기함. 어떻게 그렇게 지내는 거지? 생각 안 하고 즐기는 게 제일 좋겠지만!"},
            {"en", "It's amazing to see people who are friendly with everyone. How do they live like that? It would be best to enjoy without thinking too much!"},
            {"ja", "広く多くの人と仲良く過ごしている人を見ると不思議だ。どうやってそんな風に過ごしているんだろう？考えずに楽しむのが一番いいんだろうけど！"}
        }
    },
    {17, new Dictionary<string, string>()
        {
            {"ko", "잠에서 깨어나니 온몸이 땀에 젖었다.이상함…."},
            {"en", "Woke up with my whole body soaked in sweat. Strange..."},
            {"ja", "目が覚めると全身が汗でびっしょりだった。変だ...。"}
        }
    },
    {18, new Dictionary<string, string>()
        {
            {"ko", "평소에 꿈의 내용은 기억나지 않지만, 불쾌한 느낌만이 남아있다. 불쾌한 꿈이라도 꾼건가?"},
            {"en", "Usually I don't remember the content of my dreams, but only an unpleasant feeling remains. Did I have an unpleasant dream?"},
            {"ja", "普段は夢の内容を覚えていないのに、不快な感じだけが残っている。不快な夢でも見たのかな？"}
        }
    },
    {19, new Dictionary<string, string>()
        {
            {"ko", "근래 악몽을 꿈. 내용은 제대로 떠오르지 않지만, 보였던 것들은 몇몇개 기억난다."},
            {"en", "Having nightmares lately. Can't recall the content properly, but remember a few things I saw."},
            {"ja", "最近悪夢を見る。内容はちゃんと思い出せないが、見えたものはいくつか覚えている。"}
        }
    },
    {20, new Dictionary<string, string>()
        {
            {"ko", "나는 자취방에서 일어나서, 나가고자 문을 열고 들어간다. 어두컴컴한 곳을 지나면, 나는 똑같은 방에 서있다. 갑갑함."},
            {"en", "I wake up in my studio apartment, open the door to leave and enter. After passing through a dark place, I'm standing in the same room. Suffocating."},
            {"ja", "私はワンルームで目覚めて、出ようとドアを開けて入る。暗い場所を通り過ぎると、私は同じ部屋に立っている。息苦しい。"}
        }
    },
    {21, new Dictionary<string, string>()
        {
            {"ko", "잠에서 깰 때마다 심장을 조이는 듯한 기분이 굉장히 불쾌."},
            {"en", "Every time I wake up, the feeling of my heart being squeezed is extremely unpleasant."},
            {"ja", "目が覚めるたびに心臓を締め付けられるような気分が非常に不快だ。"}
        }
    },
    {22, new Dictionary<string, string>()
        {
            {"ko", "일을 제대로 못했다… 과제가 밀려오는데, 집중이 되지 않는다. 머릿속이 뒤엉켜 있다."},
            {"en", "Couldn't do the work properly... Tasks are piling up, but I can't concentrate. My mind is tangled."},
            {"ja", "仕事をちゃんとできなかった...課題が積み重なっているのに、集中できない。頭の中がごちゃごちゃだ。"}
        }
    },
    {23, new Dictionary<string, string>()
        {
            {"ko", "음악도 제대로 듣기 힘들다. 비효율적이다. 자면 좋아지지 않을까 싶지만, 잠을 잘 때마다 악몽."},
            {"en", "Even listening to music is difficult. It's inefficient. I think it might get better if I sleep, but I have nightmares every time I sleep."},
            {"ja", "音楽もちゃんと聴けない。非効率的だ。寝れば良くなるかと思うけど、寝るたびに悪夢。"}
        }
    },
    {24, new Dictionary<string, string>()
        {
            {"ko", "어릴 때 할머니의 모습이 계속해서 떠오른다. 몸이 갑갑하고, 어두운 생각이 떠오른다."},
            {"en", "The image of my grandmother from childhood keeps coming to mind. My body feels stuffy, and dark thoughts arise."},
            {"ja", "子供の頃の祖母の姿が頭に浮かび続ける。体が息苦しく、暗い考えが浮かんでくる。"}
        }
    },
    {25, new Dictionary<string, string>()
        {
            {"ko", "어릴 때나, 한번씩 꿈에서 너무 겁나는 일, 불쾌한 일, 소름끼치는, 공포스런 일을 겪으면 오열할 때가 있는데."},
            {"en", "When I was young, sometimes in dreams I'd experience terrifying, unpleasant, chilling, horrifying things and end up wailing."},
            {"ja", "子供の頃、時々夢で怖いこと、不快なこと、ぞっとするような恐ろしいことを経験して泣き叫ぶことがあった。"}
        }
    },
    {26, new Dictionary<string, string>()
        {
            {"ko", "일어나고 보면, 그러다가 반 쯤깨서, 실제로 울었던 흔적이 남아있기도 하고. 요새 자꾸 그런 것 같아."},
            {"en", "When I woke up, I'd find traces of having actually cried, half-awake. It feels like that's happening a lot lately."},
            {"ja", "起きてみると、半分目が覚めた状態で実際に泣いた跡が残っていることもある。最近またそんな感じがする。"}
        }
    },
    {27, new Dictionary<string, string>()
        {
            {"ko", "너무 찌뿌둥하고, 갑갑하다. 머리가 어지럽고, 뒤죽박죽이고, 울렁거린다. 멍한, 일이 손에 잡히지 않는, 그런 느낌."},
            {"en", "I feel so heavy and suffocated. My head is dizzy, everything's a mess, and I feel nauseous. It's a dazed feeling, like I can't get a grip on anything."},
            {"ja", "体がだるく、息苦しい。頭がくらくらして、めちゃくちゃで、吐き気がする。ぼんやりして、何も手につかない、そんな感じ。"}
        }
    },
    {28, new Dictionary<string, string>()
        {
            {"ko", "첼로를 킬 때, 할머니 생각이 떠오름. 그 일이 있기 전에, 부모님이 바쁘실 때, 할머니 손에서 컸으니까."},
            {"en", "When I play the cello, thoughts of my grandmother come to mind. Before that incident, when my parents were busy, I grew up under my grandmother's care."},
            {"ja", "チェロを弾くとき、祖母のことを思い出す。あの出来事の前、両親が忙しかったとき、祖母の手で育ったから。"}
        }
    },
    {29, new Dictionary<string, string>()
        {
            {"ko", "어찌 보면 첼로도 할머니를 본받았던 게 아닌가. 감사하지만, 그 일이 있고, 할머니가 돌아가시고. 조금 많이, 힘들었다."},
            {"en", "In a way, maybe I took after my grandmother in playing the cello too. I'm grateful, but after that incident, and after grandmother passed away. It was quite difficult."},
            {"ja", "ある意味では、チェロも祖母から学んだのかもしれない。感謝しているけど、あの出来事があって、祖母が亡くなって。少し、いや、かなりつらかった。"}
        }
    },
    {30, new Dictionary<string, string>()
        {
            {"ko", "근래 꿈도 그렇고, 피곤하거나, 무기력하거나, 스스로 소스라칠 때가 좀 잦은데. 몸 상태 안 좋은 것과도 연관이 있나 싶기도..."},
            {"en", "Lately, with the dreams and all, I often feel tired, listless, or startled. I wonder if it's related to my poor physical condition..."},
            {"ja", "最近の夢もそうだし、疲れていたり、無気力だったり、自分でびくっとすることが多いんだ。体調が悪いこととも関係があるのかな..."}
        }
    },
    {31, new Dictionary<string, string>()
        {
            {"ko", "첼로를 제대로 키지 못했다. 지양하고자 했지만, 평소 약간은 업신 여긴 대상에게 치욕적인 순간이 생긴다는 것은 정말 정말 별로…"},
            {"en", "I couldn't properly cultivate my cello skills. Although I tried to avoid it, it's really awful to have humiliating moments with someone I usually looked down on a bit..."},
            {"ja", "チェロをちゃんと育てられなかった。避けようとしたけど、普段少し見下していた相手に恥ずかしい瞬間が生まれるのは本当に最悪だ..."}
        }
    },
    {32, new Dictionary<string, string>()
        {
            {"ko", "몸, 정신 둘 다 요새 너무 힘들다. 힘든 일이라면 꽤 많이 있어왔다고 생각했는데."},
            {"en", "Both my body and mind are so tired these days. I thought I'd been through quite a lot of tough times."},
            {"ja", "体も心も最近とてもつらい。辛いことはかなりたくさんあったと思っていたけど。"}
        }
    },
    {33, new Dictionary<string, string>()
        {
            {"ko", "요새는 오히려 힘들어서, 더 힘든. 내가 컨디션이 좋지 않기에 그러한 일들이 더 많이 일어나는 것 같아."},
            {"en", "But these days, it's harder because it's hard. It seems like more of these things are happening because I'm not in good condition."},
            {"ja", "最近はむしろ辛くて、より辛い。自分のコンディションが良くないからそういうことがより多く起こっているような気がする。"}
        }
    },
    {34, new Dictionary<string, string>()
        {
            {"ko", "힘든 일도 딱히 없는 요즘은 대체 왜 그럴까?"},
            {"en", "Why is it like this even when there's nothing particularly difficult going on these days?"},
            {"ja", "特に辛いこともない最近は一体なぜだろう？"}
        }
    },
    {35, new Dictionary<string, string>()
        {
            {"ko", "이제 완전히 하지 못하겠다. 첼로를 키려 하거나, 내가 평소에 하던 음악을 들으면 피가 식는 것같아."},
            {"en", "I can't do it anymore. When I try to play the cello or listen to the music I used to, it feels like my blood runs cold."},
            {"ja", "もう完全にできない。チェロを弾こうとしたり、普段聴いていた音楽を聴くと血が凍るような気がする。"}
        }
    },
    {36, new Dictionary<string, string>()
        {
            {"ko", "이전에 할머니와 함께 시간을 보냈을 때. 말에, 할머니께서 이상해지셨을 때. 그떄랑 비슷하다."},
            {"en", "It's similar to when I spent time with my grandmother before. When grandmother started acting strange. It's similar to that time."},
            {"ja", "以前、祖母と時間を過ごしていたとき。祖母がおかしくなったとき。あの時と似ている。"}
        }
    },
    {37, new Dictionary<string, string>()
        {
            {"ko", "할머니께서도 처음엔 악몽. 그 이후엔 점점 상태가 안 좋아지셨고. 나랑 비슷한 두통이나, 오한 등을 호소하시다, 바로 이상해지셨었으니까."},
            {"en", "Grandmother also had nightmares at first. After that, her condition gradually worsened. She complained of headaches and chills similar to mine, and then suddenly became strange."},
            {"ja", "祖母も最初は悪夢だった。その後、徐々に状態が悪くなっていって。私と似たような頭痛や寒気を訴えていたかと思うと、すぐにおかしくなってしまったんだ。"}
        }
    },
    {38, new Dictionary<string, string>()
        {
            {"ko", "어릴 때 할머니와 같이 시간을 보낼 떄 였다. 음악이나 첼로에 관심을 갖게된 것도 그때였고."},
            {"en", "It was when I spent time with my grandmother as a child. That's also when I became interested in music and the cello."},
            {"ja", "子供の頃、祖母と一緒に過ごしていた時だった。音楽やチェロに興味を持ったのもその頃だ。"}
        }
    },
    {39, new Dictionary<string, string>()
        {
            {"ko", "좋은 쪽으로나 나쁜 쪽으로나 큰 영향이 있었다는 점은 부정할 수 없겠다."},
            {"en", "I can't deny that it had a big influence, both good and bad."},
            {"ja", "良い面でも悪い面でも大きな影響があったことは否定できないだろう。"}
        }
    },
    {40, new Dictionary<string, string>()
        {
            {"ko", "어느 순간부터 할머니께선 이상해지셨다. 잠을 자고 일어나시기만 하면, 소리를 지르시거나, 바로 쇠약해져 가는 게 눈에 보였다."},
            {"en", "At some point, grandmother started acting strange. Every time she woke up, she would scream or visibly weaken right away."},
            {"ja", "ある時から、祖母はおかしくなり始めた。寝て起きるだけで叫んだり、すぐに衰弱していくのが目に見えた。"}
        }
    },
    {41, new Dictionary<string, string>()
        {
            {"ko", "어지러워 하시거나. 그때는 단순히 나이가 드셔서 그런 것인 줄 알았는데…"},
            {"en", "Or she would feel dizzy. At that time, I thought it was simply because she was getting old..."},
            {"ja", "めまいを訴えたり。その時は単に年を取ったからだと思っていたけど..."}
        }
    },
    {42, new Dictionary<string, string>()
        {
            {"ko", "부모님께서는 두 분 다 바쁘셔서 할머니의 상태를 제대로 파악하지 못하였다. 그래도 그 날 전까지는 정상적인 생활이 가능하셨으니까."},
            {"en", "My parents were both too busy to properly understand grandmother's condition. Still, until that day, she was able to live normally."},
            {"ja", "両親は二人とも忙しくて、祖母の状態をきちんと把握できなかった。それでもあの日まではまともな生活ができていたのだから。"}
        }
    },
    {43, new Dictionary<string, string>()
        {
            {"ko", "하지만 어느 순간부터는 그것조차 안 되셨고. 하루 종일 소리를 지르시거나, 괴로워 하시거나, 정말 모든 것이 고장나버린."},
            {"en", "But from some point on, even that became impossible. She would scream all day, or be in pain, as if everything had broken down."},
            {"ja", "でもある時からそれさえもできなくなって。一日中叫んだり、苦しんだり、本当にすべてが壊れてしまったかのように。"}
        }
    },
    {44, new Dictionary<string, string>()
        {
            {"ko", "존경의 대상이었던 할머니가 보이는 그런 모습. 한순간에 사라져 버린 보호자. 오히려 어렸던 내가 할머니를 보호해야 했던 상황은 남아있다."},
            {"en", "Seeing my grandmother, who was an object of respect, in that state. A guardian who disappeared in an instant. The situation where I, young as I was, had to protect my grandmother remains."},
            {"ja", "尊敬の対象だった祖母がそんな姿を見せる。一瞬にして消えてしまった保護者。むしろ幼かった私が祖母を守らなければならなかった状況が残っている。"}
        }
    },
    {45, new Dictionary<string, string>()
        {
            {"ko", "그때는 온갖 부정적인 생각밖에 들지 않았지만, 지금 생각해보면 그런 생각이 든다."},
            {"en", "At that time, I could only think negatively, but now when I think about it, these thoughts come to mind."},
            {"ja", "当時はネガティブな考えしか浮かばなかったけど、今考えるとこんな思いが浮かぶ。"}
        }
    },
    {46, new Dictionary<string, string>()
        {
            {"ko", "일기를 쓸 때 만큼은 어느 정도 괜찮지만, 밖에 잘 나갈 수도 없고, 이제까지 해왔던 음악을 하지 못하며,이로써 모든 생산적인 활동을 할 수 없지 않은가."},
            {"en", "I'm somewhat okay when writing in my diary, but I can't go out much, can't do the music I've been doing until now, and thus can't do any productive activities."},
            {"ja", "日記を書いている時はある程度大丈夫だけど、外にもあまり出られないし、今までやってきた音楽もできず、これではすべての生産的な活動ができないじゃないか。"}
        }
    },
    {47, new Dictionary<string, string>()
        {
            {"ko", "친구는 없고, 부모님도 지금은 없다. 사람과의 관계, 실력 모두 잃은 내게 어떤 가치가 있는가."},
            {"en", "I have no friends, and my parents are gone now. What value do I have, having lost all relationships and skills?"},
            {"ja", "友達はいないし、両親も今はいない。人との関係も、能力もすべて失った私にどんな価値があるのだろうか。"}
        }
    },
    {48, new Dictionary<string, string>()
        {
            {"ko", "이런 글을 쓰는 것은, 미치지 않았다는 것에 대한 반증. 나는 말 할 수 있고. 숨쉴 수 있다."},
            {"en", "Writing this is proof that I haven't gone crazy. I can speak. I can breathe."},
            {"ja", "こんな文章を書くことは、私が狂っていないことの証明だ。私は話すことができる。呼吸ができる。"}
        }
    },
    {49, new Dictionary<string, string>()
        {
            {"ko", "아무도 없다면 나도 살아갈 수 있을 것 같은데. 모두 사라지기 보단, 나 스스로가 사라져야 하는 게 아닐까."},
            {"en", "If there's no one else, I think I could live on. Rather than everyone disappearing, shouldn't I be the one to disappear?"},
            {"ja", "誰もいなければ私も生きていけそうなのに。みんなが消えるよりも、私自身が消えるべきなんじゃないだろうか。"}
        }
    },
    {50, new Dictionary<string, string>()
        {
            {"ko", "첼로를 키지 못하는 내게 남아있는 게 무언가. 음악 소리를 들을 때 마다, 막연한 불안감. 갑갑함이 심하게 몰려온다."},
            {"en", "There's something left in me who can't play the cello. Every time I hear music, a vague anxiety. A severe feeling of suffocation comes over me."},
            {"ja", "チェロを弾けない私に残っているものは何か。音楽の音を聞くたびに、漠然とした不安感。息苦しさが激しく押し寄せてくる。"}
        }
    },
    {51, new Dictionary<string, string>()
        {
            {"ko", "오래된 앨범을 발견했다. 할머니와 찍은 사진들. 할머니도 비슷했을 것 같다."},
            {"en", "I found an old album. Photos taken with grandmother. Grandmother probably felt similar."},
            {"ja", "古いアルバムを見つけた。祖母と撮った写真たち。祖母も似たような思いだったのかもしれない。"}
        }
    }
};

    #endregion

    #region Basic UI

    public Dictionary<string, Dictionary<string, string>> GameUITranslations = new Dictionary<string, Dictionary<string, string>>()
    {
        {"GameStart", new Dictionary<string, string>()
            {
                {"ko", "게임 시작"},
                {"en", "Start Game"},
                {"ja", "ゲーム開始"}
            }
        },
        {"Settings", new Dictionary<string, string>()
            {
                {"ko", "설정"},
                {"en", "Settings"},
                {"ja", "設定"}
            }
        },
        {"Exit", new Dictionary<string, string>()
            {
                {"ko", "종료"},
                {"en", "Exit"},
                {"ja", "終了"}
            }
        },
        {"Language", new Dictionary<string, string>()
            {
                {"ko", "언어"},
                {"en", "Language"},
                {"ja", "言語"}
            }
        }
    };

    #endregion

    #region Tutorial

    public Dictionary<string, Dictionary<string, string>> TutorialTranslations = new Dictionary<string, Dictionary<string, string>>()
    {
        {"TutorialMove", new Dictionary<string, string>()
            {
                {"ko", "WASD 를 통해 이동할 수 있습니다."},
                {"en", "You can move using WASD keys."},
                {"ja", "WASDキーで移動できます。"}
            }
        },
        {"TutorialInteract", new Dictionary<string, string>()
            {
                {"ko", "표식이 나타날 경우 E 버튼을 눌러 상호작용 할 수 있습니다."},
                {"en", "When a marker appears, press the E button to interact."},
                {"ja", "マーカーが表示されたら、Eボタンを押して相互作用できます。"}
            }
        },
        {"TutorialHealth", new Dictionary<string, string>()
            {
                {"ko", "공포 이벤트를 보면, 체력이 감소합니다. 게임의 플레이 횟수에 따라 최대 체력이 늘어납니다."},
                {"en", "When you encounter a fear event, your health decreases. Your maximum health increases based on the number of times you've played the game."},
                {"ja", "恐怖イベントに遭遇すると、体力が減少します。プレイ回数に応じて最大体力が増加します。"}
            }
        }
    };

    #endregion

    private void Start()
    {
        AddTranslationsToTable(ScriptTranslations, "StoryTranslations");
        AddTranslationsToTable(GameUITranslations, "GameUITranslations");
        AddTranslationsToTable(TutorialTranslations, "TutorialTranslations");
    }
    
    void AddTranslationsToTable(Dictionary<int, Dictionary<string, string>> translations, string tableName, string keyPrefix = "", bool useIndex = false, int startIndex = 1)
        {
            int currentIndex = startIndex;

            foreach (var entry in translations)
            {
                string key;
                if (useIndex)
                {
                    key = $"{keyPrefix}{currentIndex}";
                    currentIndex++;
                }
                else
                {
                    key = string.IsNullOrEmpty(keyPrefix) ? entry.Key.ToString() : $"{keyPrefix}{entry.Key}";
                }

                foreach (var languageEntry in entry.Value)
                {
                    string localeCode = languageEntry.Key;
                    string translatedText = languageEntry.Value;

                    // 해당 로케일에 대한 Locale 객체를 가져옵니다.
                    Locale locale = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(localeCode));
                    if (locale != null)
                    {
                        // 해당 로케일에 대한 테이블을 가져옵니다.
                        var table = LocalizationSettings.StringDatabase.GetTable(tableName, locale);
                        if (table != null)
                        {
                            // Editor 상에서 변경사항을 저장하기 위해 Dirty 플래그를 설정합니다.
                            EditorUtility.SetDirty(table);
                
                            // 키가 이미 존재하는지 확인하고, 없으면 추가하거나 업데이트합니다.
                            var tableEntry = table.GetEntry(key);
                            if (tableEntry == null)
                            {
                                table.AddEntry(key, translatedText);
                            }
                            else
                            {
                                tableEntry.Value = translatedText;
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"Table '{tableName}' for locale {localeCode} not found.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Locale for code {localeCode} not found.");
                    }
                }
            }

            // 변경사항을 저장합니다.
            AssetDatabase.SaveAssets();
        }

    void AddTranslationsToTable(Dictionary<string, Dictionary<string, string>> translations, string tableName, string keyPrefix = "", bool useIndex = false, int startIndex = 1)
        {
            int currentIndex = startIndex;

            foreach (var entry in translations)
            {
                string key;
                if (useIndex)
                {
                    key = $"{keyPrefix}{currentIndex}";
                    currentIndex++;
                }
                else
                {
                    key = string.IsNullOrEmpty(keyPrefix) ? entry.Key : $"{keyPrefix}{entry.Key}";
                }

                foreach (var languageEntry in entry.Value)
                {
                    string localeCode = languageEntry.Key;
                    string translatedText = languageEntry.Value;

                    // 해당 로케일에 대한 Locale 객체를 가져옵니다.
                    Locale locale = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(localeCode));
                    if (locale != null)
                    {
                        // 해당 로케일에 대한 테이블을 가져옵니다.
                        var table = LocalizationSettings.StringDatabase.GetTable(tableName, locale);
                        if (table != null)
                        {
                            // Editor 상에서 변경사항을 저장하기 위해 Dirty 플래그를 설정합니다.
                            EditorUtility.SetDirty(table);
                
                            // 키가 이미 존재하는지 확인하고, 없으면 추가하거나 업데이트합니다.
                            var tableEntry = table.GetEntry(key);
                            if (tableEntry == null)
                            {
                                table.AddEntry(key, translatedText);
                            }
                            else
                            {
                                tableEntry.Value = translatedText;
                            }
                        }
                        else
                        {
                            Debug.LogWarning($"Table '{tableName}' for locale {localeCode} not found.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Locale for code {localeCode} not found.");
                    }
                }
            }

            // 변경사항을 저장합니다.
            AssetDatabase.SaveAssets();
        }


    
}
