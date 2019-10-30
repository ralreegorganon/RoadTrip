using System.Drawing;
using System.IO;
using Leopotam.Ecs;
using RoadTrip.Game.Components;

namespace RoadTrip.Game
{
    public class Game
    {
        public bool Run { get; set; } = true;

        public Map Map { get; set; } = new Map();

        public EcsEntity Player { get; private set; }

        public EcsEntity Cursor { get; private set; }

        private EcsWorld World { get; }

        private EcsSystems Systems { get; }

        public Game(EcsWorld world, EcsSystems systems)
        {
            World = world;
            Systems = systems;
        }

        public void Setup()
        {
            Player = World.NewEntityWith<Position, Renderable, PlayerControllable, CameraFocusTag>(out var playerPosition, out var playerRenderable, out _, out _);
            playerPosition.Coordinate = new Coordinate(0, 0, 0);
            playerRenderable.Color = Color.Red;
            playerRenderable.Symbol = '@';

            Cursor = World.NewEntityWith<Position, CursorTag>(out var cursorPosition, out _);
            cursorPosition.Coordinate = new Coordinate(0,0,0);

            Map = ExtremeHacks();
        }

        public void Tick()
        {
            Systems.Run();
            World.EndFrame();
        }

        private static Map ExtremeHacks()
        {
            var overmap = @"
...............FFFFFFFFF..FFFFF...............FFFFFFFFFFFFFFFF.......FFFFFFFFFFFFFF............................FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF......FFF..┌────────────────────┘FFFFF
.............│FFFFFFFFF....FFF......FFF.......FFFFFFF..FFFFFF........FFFFFF.FFFFFFF.............................FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFFFFFFFFFFFFFFFFF│FFFFF
.............└─┐FFFFFFF.........FF..FFFFFFFF..F.F..>o##FFFF.FF.┌──┐..FFFFFFF.FFFFFF.........................M...FFFFFFFFFFFFFFFFFFFFFFFFF>──┌────────────┘FFFFFFFFFFFFFFFFFFFF└<FFFF
.............FF└┐FFFFFFFF..F....F.FFFFFFFFFFFFF..┌─#o##..FFFF.┌┘FF│...FFFFFF.FFFF............................FFFFFFFFFFFFFFFFFFFFFFFFFFFFF┌─┘FFFFFFFFFFFFF.FFFFFFFFFFFFFFFFFFFFFFFFF
............FFFF├─────┐FFFFFF......FFFFFFFFFFFFF┌┘.##&#....FFT│FFF│.FFFF..FFFFFFF..........................FFFFFFFFFFFFFFFFFFFFFFFFFFF┌───┘FFFFFFFFFFFFFF.~FFFFFFFFFFFFFFFFFFFFFFFFF
..FFF.......FFFF│FFFFF│FFFFFFFF....FFFFFFFFFF┌──┘..###o.....FTFFFF│FFFF....CFFFFF..........................FFFFFFFFFFFFFFFFFFFFFFFFF┌─┘FFFFFFFFFFFFFFFFF...FFFFFFFFFFFFFFFFFFFFFFFF.
FFFFFF.......FFF│.FFF─┴─┬┬────┐....FFFFFFFFF┌┘FFF...........FF..FF└┐FF....FFFFFFF........................FFFFFFFFFFFFFFFFFFFFFFFFFF┌┘FFF..FFFFFFFFFFFFFFFFFFFFFFFFFF.FFFFFFFFFFFFF.F
FFFFFFF┌─┐......TFFFFFFF└┤FFFF└──T─┐..FFFFFF│FFF............FFF.FFF└──┐..FFFFFFFFF...FFFFFFF.............FFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF.
FFFFFFF│F└─┐....│.FFFFFFF│F.FFF....└┐...FF┌─┘FFF..........FFFFF.FFFFFF└┐FFFFFFFFF....FFFFFFFF............FFFFFFFFFFFFFFFF┌──┐FFFFF┌┘FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF.
FFFFFFF├###│....│FFFF....│F%........└┬┬───┘FFFFF..........FFF....FFFFFF│FFFFFFFFFFFFFFFFFFFFF..........FFFFFFFFFFFFFFFFFF^FF│FFFFF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF.FF.
FFFFFFFO###└┐...└┐FF.....│FF.........├┘..FFFFFFF........FFFFFF....FFFFF└┐FFFFFFFFFFFFFFFFFFFF........FFFFFFFFFFFFFFFFFFFFFFF└───┬─┘FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF..FFF
FFFFFFF.....├──┐.└──────┐│...........│...FFFFFFFFF......FFFFF......FFFFF│FFFFFFFFFFFFFFFFFFFFF...FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF..F.F.
FFFFF......┌┘..└┐.......│T..........┌┘....FFFFFFFFFFFFF.FFFFFF.....FFFFF└┐FFFFFFFFFFFFFFFFFFFFF.FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF..FFFFFFFFFFFF...
FFFF.......│....│.......├─..........│......FFFFFFFFFFFFFFFFFFFF...FFFFFFF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFRRR│RFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF....FFFFFFFFFFF....
FFF.......┌┘....└──┐...┌┘..........┌┘.........FFFFFFFFFFF.FFFFFFF...FFFFF└┐FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFRRR│RRFFFFFFFFFFFFFFFFFFFFFFFFFFFF.......FFFFFFFFFFF...
..F....┌──┘........└┐..│...........│..FFF.....FFFFFFFFFFF..FFFFFFFFFFFFFFF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFRRR│RRFFF..FFFFFFFFF.FFFFFFFFFFFF.........F..FFFFFFFFF.
..XX┌──┘............│..│..........┌┘...F........FFFFFFF....FFFFFFFFFFFFFFF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFRRR│RRRRF.FFFFFFFFFFFFFFFFFFFFFF.............FFFFF.FFFF
..XX│...............└──┤..........│..............FFFFFF....FFFFFFFFFFFFFFF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFRRR│RRRRF.FFFFFFFFFRRRRRFFFFFFFF...............FFFFFFFF
..└─┘..................└──┐.......│.............FFFFFFFF...FFFFFFFFFFFFFFF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF..FFFFFFFFFFFRRRFFRRRR│RRRRRR.FFFFFFFRRRRRRRRRRFFFF...............FFFFFFFF
..........................└───┐.TT│...........FFFFFFFFFFF...FFFFFFFFFFFFFF│FFFFFFFFFFFFFFFFFFFFFFFFFFF..FF..FFFFFFFFFFRRRRRRRRRR│RRRRRRRRRFFFFFRRRRRRRRRRRFFF..............FFFRRRFFF
..............................└─┬─┘............FFFFFFFFF......FFFFFFFFFFFF│FFFFFFFFFF..FFFF.FFFFFFFFF.......FFFFFFFFRRRRRRRRRRRR│RRRRRRRRRRFFFFRRRRRRRRRRRFF........RRRRR..FFFRRRRFF
................................│................FFFFFFF......FFFFFFFFFFFF│FF.FF┌─┬┬──┐......FFF.FF.............FFRRRRRRRRRRRRRR│RRRRRRRRRRRRRRRRRRRRRRRRR.......FF.RRRRRRRRRRRRRRRR
................................│................FFFFFF.......FFFFFFFFFFFF└─────┘.│OR││......FFF..F............RRRRRRRRRRRRRRRRR│.RRRRRRRRRRRRRRRRRRRRRRRRR......FRRRRRRRRRRRRRRRRRR
................................│................FFFFF.F....FFFFFFFFFFFFF.........│OR│└─┐F.F.FF.....F..........RRRRRRRRRRRRRRRF┌┘..RRRRRRRRRRRRRRRRRRRRRRRRR..F.FFRRRRRRRRRRRRRRRRRR
................................│................FFFF....F..FFFF.F.FFFFF..........│OR│..└─┐...F....FF..........RRRRRRRFFRRRRRRF│FFFRRRRRRRRRRRRRRRRRRRRRRRRRR.FFFRRRRRRRRRRRRRRRRRRR
................................│.....................X...FFFFFF...FFFFFF.........│OR│....└─┐....FFFFF........RRRRRRRRFFFFFFFFF│FFFRRRRRRRRRFFFF.RRRRRRRRRRRRRRFFRRRRRR.│FFFFFFFFRRR
................................│............................RRR..................├┘RRRRRR..└┐.FFFFFFFF.....RRRRRRRR..FFFFFFFFF│FFFFRRRRRRRR..FFFRRRR..RRRRRRRRRRRRRRRFF│FFFFFF.F─┐F
................................│...........................RRRRR...............┌─┘.RRRRRR...│.FFFFFFF......RRRRRRRRR..FFFFFFFF│FFFFFFFFRRRR.FF.FFF.....RRRRRRRRRRRRRR┌─┤FFFFFFFFF│F
................................│...........................RRRRRR.............R│R..RRRRRR...│....FFFRRRR...RRRRRR+RRRR.FFFFFFF│FFFFFFFFFFF..F...FF.....FF..RRRRRRRRRR│F└────┐FFF┌┘F
RRR.............................│..........RRR..............RRRRRR......RRRRRRRR│RRRRRRRRRRR.└──┐.RRRRRRRR..FRRRRRRRRRRRFFFRRRR│FFFFFFFFFFF.FFF....F..FFFFFFRRRRRRRRRF│FFFFFF│FF┌┘FF
RRR.............................│........RRRRRR...........RRRRRRRRRRRRRRRRRRRRRR│RRRRRRRRRRRRRRR│RRRRRRRRRR.FRRRRRRRRRRRRwwRRRR│FFFFFFFFFFF..FF....FFFFFFFFFRRRRRRFF┌─┘FFFFFF└──┘FFF
RRR.......RRR...FFF.........RRRR│......F.RRRRRR...........RRRRRRRRRRRRRRRRRRRRRR│RRRRRRRRRRRRRRR│RRRRRRRRRR.FRRRRRRRRRRRRRRRRRR│FFFFFFFFFFFF..FF.....FFFFFFFFFRRRFF┌┘FFFFFFFFFFFFFFF
RRR.....RRRRRFRRRRF.......RRRRRR│R.....F.RRRRRR....RRRRR..RRRR.RR+RRRRRRRRRRRRRR│RRRRRRRRRRRRRRR│RRRRRRRRRR..RRRRRRRRRRRRRRRRRR│FFFFFFFFFFF.....F....FFFFFFFFFFF┌──┘FFFFFFFFFFFFFFFF
RRRRRRRRRRRRRFRRRRRR......RRRRRR│RR..RRRRRRRRRRR.RRRRRRR.RRRRR..RRRRRRRRRRRRRRRR│RRRRRR──┼─RRRRR│RRRRRRRRRR..RRRRRRRRRRRRRRRRRR│FFFFFFFFFFF...........FFFFFFFFF┌┘FFFFFFFFFFFFFFFFFFF
RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR│RRRRRRRRRRRRRRR.RRRRRRRRRRRRR...RRRR...RRRRRRR.└┐RRRR..w│s....n│RRRRRRRRRRR..RRRRRRRRRRRRRRRRR│FFFFFFFFFFF............FFFFFFFF│FFFFFFFFFFFFFFFFFFF.
RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR│RRRRRRRRRRRRRRRRRRRRRRRRRRRRR..........RRRRRRR..│...vvvC│<v...nRRRRRR..RRRRR.RRRRR..RRRRRRRRR┌┘FFFFFFFFFFF............FFFFFFF┌┘FFFFFFFFFFFFFFFFF...
.RR...RRR│FRRRRRRRRRRRRRRRRRRRRR│.RRRRRRRRRRRRRRRRRRRRRRRRRRR.............┌──────┴───────┼─────┐RRRRR....RRRRRRRRR...RRRR┌────┘FFFFFFFFFFFF##..........FFFFFFF│FFFFFFFF┌─0FFFFF.....
..FFF│F┌─┘FFRRR.RRRF..X.RRRRRRR┌┘.RRRRRRFFRRRRRRRRRRRRRRRRRRR............┌┘FFFFF.....^^.^│O^...│RRRRR....RRRRRRRR....RRRR│..FFFFFFFFFFFFFFF##..........FFFFFFF│FFFFFFFF│FFFFFFF.....
.FFFF├─┘FFFFFF┌───────┤..RRRR..│.RRRRRRRFFFRRRRRRRRRRRRRR..............┌─┘FFFFFF.....H..>│<....│RRRRR....RRRRRRR......RRR│..FFFFFFFFFFFFFFF##.........FFFFFFFF│FFFFFFFF│FFFFFFF.....
FFFFF│FFFFF..F│FFFFFFF│........│.RRRRRRR.FFFFFRRRRRRRRRRR..............│.FFFFFFF...............└─┬───────┐RRRRR.......┌──┘.FFFFFFFFFFFFFF┌─┴O..........FFFFFFF│FFFFFFFF│FFFFFF......
FFFFF│FFF.┌─┬─┘FFFFFFF│........│+RRRRR.......FRRRRRRRF.................│FFFFFFFF.................│.......└────┬───────┤F.FFFFFFFFFFFFFFF┌┘F..................F└┌──┐F─┬┌┘FFFFF.......
FFFFF│FFFF│L│...FFFFFF│........└┤RRR.........FRRRRRRFFF..............┌─┘FFFFFFFF.................│......FF....│.......└┐FFFFFFFFFFFFFFF┌┘FFF..................F│└┐└───┘FFFFF........
FFFFF│FFF...│....FFFFF└┐........└┐...........FFFFF.FFFFF.............│.FFFFFFFFF.................└┐......FF...└L.......│FFFFFFFFFFFFFF┌┘FFF..................┌─┘F└┬─┬┤FFFFF.........
FFFFF│FFF...└┐...FFFFFF│.........│............┌─SSFFFFF..............│FFFFFFFFF...................│.....FF.............└┐FFFFFFFFFF.F.│.FFF..................│.FFF│F│└┐FFFF.........
FF───┤F......│....FFFFF│........>│F..........┌┘FFFFFFF...............│.FFFFFFFF...................│.....F...............└┐FFFFFFFFF.F.│..FFF.................│FFFFFF│F│FFFF........F
FFFFF│FF.....│.....FFFF│........>│<F......┌──┘.FFFFFFFFFF.........C..│..FFFFFF....................└┐.....................├───┐FFFFF.FF│FFF...................│FFFFFF│FFFFFFF...FFFFF
FF.F┌┴──┐....└┐.....FFF└────┐...F│<F...┌──┘....FFFFFFFFFFF........┌──┘...FFF.......................└┐....................│...└────────┴────────────┬─────────┴┐FFFFF│FFFFFFFFFFFFFFF
FF.F│FFF└┐....└┐FFF.FFF.....└─┐.F│Ov...│.┌───────┐FFFFFFFF.......┌┘.................................│....................│.........................│..........└┐FFFF│FFFFFFFFFFFFFFF
FFF.│FFFF└───┐F└+FF.F.........├──┼──┐<.│.│MMMMMMM│FFFFFFF........│........F.........................X....................│.........FF.......F......│...........└──────────┐FFFFFFFFF
FFF┌┘FFFFFFFF└─┐FFF..........>│<>│^>│<.│C│MMMMMMMMFFFFFFFF....┌──┘........FFF.......................................FF...│.......FFFF..............└┐.....888.....FF│FFFFF└┐FFFFFFFF
FFF│FFFFFFFFFFF│FF...........>│<.hhF│.>│<│MMMMMMMMFFFFFFFFF..┌┘...........FFFFF.....................................FFF..│.......FFFFFFFF...........└┐....888.....FF│FFFFFF└─┐FFFFFF
FFF│FFFFFFFFFFF│FF...........v│vvhh.│v>├─┤MMMMMMMMFF──┐FFFF..│............FFFFF.........................F.......F.FFFFFF.├─────┐..FFFFFFFF...........│....888.....F┌┘FFFFFFTT│FFFFFF
FFF│FFF.....FFF│FFF..........─┴──┬──┼─>│<│MMMMMM┌─F###│FFFFF┌┘.............FFF.......................F.FFFF..FFFFFFFFFFF.│.....└┐.FFFFFFFF.F.........L.......F...FF│FFFFFFFFF│FFFFFF
.FF│FFF......FF│FFFF.........^.^S│<C│^>│<│MMMMMM│OFFFF│FFFFF│...............FFFF.────................FFFFFFFFFFFFFFFFF...│......│..FFFFFFFFFF...............FFFF.F┌┘FFFFFFFFF│FFFFFF
..F│FFF.......F└┐FFFF...........>│.>│<>│.│MMMMMM│OFFFF├─────┴┐.............FFFFF.RRRR...............FFFFFFF..FFFFFFFFF┌──┘......├###FFFFFFFF................FFFFFF│FFFFFFFFFF│FFFFFF
..F│F..........F│FFFF...........>│v>│vv│v└──────┘OvvvF│FFFF..└┐............FFFFF┌OOOO┐..............FFFFFF..FFFFFFF.F┌┘.........O###FFFFFFFF................FFFFFF│FFFFFFF┌─F└┐FFFFF
..─┘F..........F├─┐FF.........┌──┴──┼──┼──┼──┬──┬──┬──┘FFFF...│............FFFFF└┐.┌─┘..............FFFFFF...FFFFFO┬─┘............FFFFFFFFFFFF..............FFFFFF├──────┬┘FFF│FFFFF
.FFFF..........F│F│F..........P.^^^X│<>│^^│<S│<^│<>│<FFFFFF...│...........FFFFFF.├─┘..............FFFFFFFF.......###..............FFFFFFFFFFFF.............FFFFFFF│FFFFFF│FFFF│>┐FFF
..FF...........F│FT...........P.....│..│.>│<.│<.│<C│<FF.F.....│..........FFFFFFFF│...............FFFFFFFF........###.............FFFFFFFFFFFF..............FFFFFFF│FFFFFF│FFFF└┐├#FF
F..............F│.│.............O.$>│v.│<v│$S│.>│F>│<FF.......└┐.........FFFF.F┌─┘............F..FFFFFFFF........................FFFFFFFFF................FFF───┬┬┘FFFFFF│FFFFF││FFF
........hh......│F│.............┌───┼──┼──┼──┤<v│<v│<vF........│........FFFFF┌─┘.............FF.FFFFFF...........................FFFFFFFF.................FFFFF.└┘FFFFFFF│FFFFF└─FFF
........└┴┐....FFF│.............│O^>│Ma│^^│<g│<─┼──┼──FF.......│...........┌─┘............T.FFFFFFFF..............................FFFFFFFF................FFFFFFFFFFFFFFF│FFFFFFFFFF
..........└────┬──┘.............│O.O│aF│F>│FM│FS│S^│F^FF.......│...........│...............F.FFFFFFF..............................FFFFFFFF.................FFFF.FFFFFFFFF│F....FFFFF
..............F│...............O│..O│F>│FF│<F│O>│<F│FFFF.......└┐..........│..................FFFFFF..............................FFFFFFFF.................F.F...FFFFFFFF│FF..FF..FF
.............F.│................│.Fd│<>│gFFFFFF>│<FFFFFF........│.......┌┬─┤.....FF..FF......FFFFFF...............................FFFFFFFF...................F...FFFFFFFF└┐FFFFF....
.............FF│................│FFFFFFFFFFFFFFFFFFFFFF.........│.......│0000.......FFF..FF.FFFFFF.................................FFFFFFF<>....................FFFFFFFFFF│FFFFF....
.............FF└┐.............┌─┘FFFFFFFFFFFFFFFFFFFFF..........└─┐...┌─┘0000......FFF.FFF..FFFFFF...................................FFFFFFF.....................FFFFFFFFF│FFFFF....
................│..HH....┌─┐.┌┘FFFFFFFFFFFFFFFFFFFFFFF............└┬──┘..........FFFFFFFFF..FFFFF.....................HH.............FFFFFFFF..................FFFFFFFFFFF│FFFFF....
................└─┐HH...┌┘F└─┘FFFFFFFFFFFFFFFFFOFFFFFF.............│.............FFFFFFFFF.FFFFF......................HH..............FFFFFFF.FF...............FFF────────┤FFFFFF...
..................└───┐.│FFFFFFFFFFFF.FF..FFOOOOOFFFF..............│..............FFF+FFFFF.FFFF.......................─┐.............FFFFFFFFFFF...............FFFFFFFFFF└────┐F..F
.....................F│.│FFFFFFFFFFF..F.....FFOOFFFFFF...FFFF......└┐..............FFFFFFFFFFFFF........................└┐...........FFFFFFFFFFFF..................FFFFFFFFFFFF└┐F..
......................└─┤.FFFFFFFFFF.........OOFFFFFFFF.FFFFF....FF.│..................FF.FFF............................└─┐...............FFFFFFF.......MMM.........FFFFFFFFFFF│FFF
........................│..FFFFFFFFFFF........FFFFFFFFFFFFFFFF....F.│......................................................│....HH.........FFFFFFF.......MMM..........FFFFFFFFFF│...
..................F.....│..FF.FFFFFFFFF.......FFFFFFFFFFFFFFFF......│...........................F.................FF.......│....HH.........FFFFFFF.......MMM...........FFFFFFFFF│F..
.........FF.....F.......│.......FFFFFFF.......FFFFFFFFFFFFFFFFFF....└┐..........................................FFFFF......│...┌──.............FFF......................FFFFFFFF└┐..
.........F.FF..F........│.....................FFFFFFFFFFFFFFFFFFF....│.......┌──┐...............................FFFFF......│..┌┘FFFF..F........FFFFF......FFF............FFF┌┐FFF└─.
..........FFFFFFF.......│.....................FFFFFFFFFFFFFFFFFFF....│......┌┘F.└┐.................................F.......├──┘FFFFFFFF.........FFFFF.....FFFFF...........FS$└┐FFFFF
........FFFFFFFFFF......│....................FFFFFFFFFFFFFFFFFFFFF...├─┐...┌┘FFF.│.........................................│FFFFFFFFFFF........FFFFFF.....FFFFF.........F.FSSF│FFFFF
........FFFFFFFFFFF.....│....................FFFFFFFFFFFFFFFFFFFF..F.│.└───┘FFF..│.........................................│FFFFFFFFFFF........FFFFFFFFFFFFFFFFF..........FFFF│FFFF.
.F......FFFFFFFFFF....┌─┤....................FFFFFFFFFvFFFFFFFFFFFFF.│..FFFFFFF..│............FF..........+..............┌─┘FFFFFFFFFFF........FFFFFFFFFFFFFFFFF..........FFFF│FFF..
FFFF│F..FFFFFFFFFF...┌┘.│......┌───┐..........FFFFFFFFFFFFFFFFFFFFFFF│..FFFFFFF..│............F.F........................│.FFFFFFFFFFFF.......FFFFFFFFFFFFFFFFFF..........FFFF└──┐FF
FFF┌┘F..FFFFFFFFF...─┘..└┐.....│┼┼┼│............FFFFFFFFFFFFFFFFFFFFF│FFFFFFFFF..│....FF.......FFF.....................vv│FFFFFFFFFFFFF.........FFFFFFFFFFFFFFFFFF........FFF....│..
FFF│FF..FFFFFFFFFF..│.F..└─────┤┼┼┼│.............FFFFFFFFFFFFFFFFFFFF│FFFFFFFF...└┐...FFFF...........................O┌──┘FFFFFFFFFFFFF....C...FFFFFFFFFFFFFFFFFFF........FFF....│..
FF┌┘FFFFFFFFFFFFFFFF└┐##F......│┼┼┼│................FFFFFFFFFFFFFFFFF│FF...FFF....│...FFFFFFF.........................│<^.FFFFFFFFFFFFFF....FFFFFFFFFFFFFFFFFFFFFF.........FF....│..
F─┤FFFFFFFFFFFFFFFFF>┤##F......└───┘...................FFFFFFFFFFFFFF│FF....F.....│....FFFFFFF.......................>│F.F.F>FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF.............│..
FF└┐FFFFFFFFFFFFFFFFF├##F.............................FFFFFFFFFFFF┌──┘F...........│.....FFFFFF.......................>│<F...FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF....FF........│..
FFF└──┐FFFFFFFFFFFF┌O┤##F..............................FFFFFFFFFF┌┘FFFFF..........│....FFFFFFF......................v>│vvvvFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFBFFF..F......FFF........│..
FFF+.F├────────────┘FFFF...............................FFFFFFFFF─┘FFFFFF..........└┐...FFFFFFF.................vv.p┌──┴─┬──FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF.....FFFFF.......│..
FFFFFF│FFFFFFFFFFFFFFFF................................FFFFFFFFFLLLLFFFFF..........│...FFFFFF............│<..>┌─┐v>│OOv>│<^FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF....FFFF........│..
FFFFFF│FFFFFFFFFFFFFFFF................................FFFFFFFFFLLLLFFFFF......┌───┘FFFFFFFFF............│<..>│<└──┼───>│FFFF.FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF....FFFF........│..
FFFFFF│FFFFFFFFFFFFFFF...................................FFFFFFFLLLLF.FFF......│FF.FFFFFFFFFF...........>│g...│TTnn│F..>│FFF....FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF...FFFFF.......│..
FFFFFF│FFFFFFFFFFFFFFF....................................FFFFFFLLLFFFFFF......│FFFFFFFFFFFF.........###v│OvvX│TTnn│.vvv│dg.....FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF....FFF........│..
FFFFFF│FFFFF│FFFFFFFFF....................................FFFFFFFFFFFF........┌┘FFFFFFFFFFFF.........────┼────┼────┼────┼──.....FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF....F.........│..
FFF─┐F│FFFFF│FFFFFFFFF....................................FFFFFFFFFFF.........│FFFFFFFFFFFF.............O│<mOm│^F^>│<O^^│<^.....FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF.....FF........├──
FFFO└┐│FFFFF│FFFFFFFFF............####.....................FFFFFFFFFF........┌┘FFFFFFFFF.............vOg>│vv$>│<v.>│vvv>│v.v....FFFFFFFFFFFFFFFFFFFFFFFFFFFF─┐FFFF.....FF........│..
FFFMF││FFFFF│FFFFFFFFF............####..........FF..........FFFFFFFFF.....FF.│FFFFFFFFF.............─────┼────┼────┴────┴─────FFFFFFFFFFFFFFFFFFFFFFFFFFFGGGG│FFFFF....FFF......┌┘..
FFFFF││FFFFF│FFFFFFFFF............##v#.........FFF.........FFFFFFFFF.......FF└─┐FFFFFFFF............C^..>│<.^d│<.CTT..^^^^^^^^FFFFFFFFFFFFFFFFFFFFFFFFFFFGGGG│FFFF....FF........│..F
.FFFF││FFFFF│FFFFF..................─┐........FFFF.lllll...FFFFFFFFF......FFFFF└─┐FFFFFF.............adOv│<vvv│<S.TTvvvvv.....FFFFFFFFFFFFFFFFFFFFFFFFFFFGGGG│FFFF..FFFF.......┌┘..F
FFFFF└┐FFFF┌┘FF......................└┐.......FFF..llLll...FFFFFFFFFF.....FFFFFFF└─┐FFFFFFFF.........────┼────┼────┬───┬─......FFFFFFFFFFFFFFFFFFFFFFFFFFFFF┌┘FFFFFFFFF.....┌──┘...F
FFFFFF│F┌──┘FFF.................FFFF..└──┐.FFFFF...ll_ll...FOFFOFFFFFFF.FFFFFFFFFFF└─┐FFFFFF..........O.^│^^O^│C.^C│..^│<.FFF..FFFFFFFFFFFFFFFFFFFFF.FFFFFFF│FFFFFFFFFFF....│^....FF
FFFFFF│┌┘FFFFFF..................FFF.....│.FFFF...._____...OOFFFFFFFFFFFFFFFFFFFFFFFF└┐FF.F.............O│Jv.>│Avv>│vxv│vvvFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFFFFFFFF...┌┘......F
FFFFF┌│┘FFFF........................F....│FF......._____..FFFOFFFFFFFFFFFFFFFFFFFFFFFF│F..............C.>├────┼────┼───┼───FFF.FFFFFF%..FFFFFFFFFFFFFFFFFFFF│vgvvFFFFFFF┌──┘........
FFFFF││F........................F........└┐F........┌┘.....FFOFOFFFFFFFFFFFFFFFFFFFFFF│.................>│<$^^│.^.M│^^>│w......FFFFFF...FFFFFFFFFFFFFFFFFFF>├────FFFFFFF│...........
FFFFF││.........................FFFFFFFF..└┐.......┌┘....FFOOFFFFFFFFFFFFFFFFFFFFFFFFF└─┐H...............│.F..│<v..│<O>│C.......FFF...FFFFFFFFFFFFFFFFFFFFF>│^^^^vvvFFFF│O..........
FFFF┌┘│........................FFFFFFFFFFF.└─┐..┌──┘.....FFFFFFFFFFFFFFFFFFFFFFFFFFFFF..└┐...............│...>├────┼──>│<.......FF.....FFFFFFFFFF###..FFFFF>│<─┬────FFFF│O..........
FFFF│F│..............┌─┐.......FFFF.FFFFFF...└─┬┘.....FF.FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF..│..............┌┘...>│^O^.│^.>│<......FFF....FFFFFFFF...###.....FFF│<>│M^^^FFFF│<..........
F┌──┘F│.............┌┘F│.......FFF...F.FF......│......FF.FFFFFFFFFFFFFFFFFFFFFFFFFFFFFF..│...........┌──┘..........│<..........FFFFF..FFFFFFFF...#v#.....vv>│vO│Mp│FvvFF│...........
F│FF┌─┘.............│F.│.......FF......F.......│.......FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF...│.>│........│........................FFFFFFFFFFFFFFF....┌─....X.┌──┼──┼──┼──┐F┌┘...........
F│..│..............┌┘F.│......F................│.......FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF>│<.│AvO....┌┘.....................X.FFFFFFFFFFFFFFF.....│....┌┘>│^>│<>│^^│<F│<│............
F│..│###..........┌┘FF.│.............FTF...│<..│.......FFFFFFFFFFFFFFFFFFFFFFFF..FFF...F>│<─┼──┬────┘.......................FFFFFFFFFF..FFF......│....│vv│s>│Os│SO│F>│S│............
F...│###..........│FFF.│.............FFFvvv│vvv│.......FFFFFFFFFFFFFFFFFFFF┌───┐......>│>│.>│sg│<...........................FFFFFFFFFF┌──────────┴────┴──┼──┴──┼─>│<>│<│............
....│#v#.........┌┘FFF.│............FFFF┌──┼───┘.......FFFFFFFFFFFFFFFFFFFF│wwF└┐....F>│>│vv│vv│vO.............................FFFFFFF│................^^│mO^A^│SFFFFFF│............
...>├──..........│FFFF.└┐...........FFF>│O>│FSS.........FFFFFOFv┌─│FFFFFFFFFwwFF└┐...FF└─┼──┼──┼──..............................FFFFF┌┘.................>│<.│<>│<FFFF┌─┘FF..........
...>│<...........│FFFFFF│..........FFFv>│gO│SSSvv.......FFFFF┴┬┴┘┌┘FFFFFFFFFFFFFF└┬───┐^>├Ww│^s│pO......................F.......FFFFF│......................└──┤g┌───┘.FFFFF........
....│<>│<.......┌┘FFFFFF└┐........FFFF──┼──┼──┬─┐...........F####│FFFFFFFFFFFFFFF.│F..└──┘ww│vv│<....................FFFF.......FFFFF│......................^.^└┬┘.....FFFFF........
..v>│v>│v.v....┌┘FFFFFFFX┘..FFFF.FFFFF^A│F^OOM│O│<...........####│FFFFFFFFFFFFFFFF│FF.....──┼──┴────┐................FFFF........F.┌─┘..........................│......FFFF.........
..──┼──┼────..┌┘FFFFFFFFFF.FFFFFFFFFFFFg│<...>│g│<...............└┐....FFFFFF..┌──┘FFF.F..^^│C^^....└────┐...........FFFFF.....FFFF│............................│...................
..^^│<g│<^^^..│FFFFFFFFFFFFFFFFFFFFFFFFp│<......└──┐..............├────────────┘..FFFFFF...>│<...........└────┐......FFFFFFFFF.FFFF└┐...........................│...................
...>│<>├──┐...│FFFFFFFFFFFFFFFFFFFFFFFF┌┘.......MMM└┐.............│F..FFFFFFFFF...FFFF......│..+..............└──────┐FFFFFFFFFFFFFF│...........................│...................
.AA>│<>│^>│x.┌┘FFFFFFFFFFFFFFFFFFFFFF.F│........MMM.│.......F┌────┘FFFFFFFFFFFFF...FFFF.....│.......................F└──────┐FFFFFFF│...........................│...................
.AA>│<>│<v│vS│<FFFFFFFFFFFFFFFFFFFFFF┌─┘........MMM.└┐....┌──┘....FFFFFFFFFFFFFFFFHFFF......│.......................FFFFFFFF└───────┤...........................│...................
F.──┼──┼──┼──┤FFFFFFFFFFFFFFFFFFFFFF┌┘FF.............│....│.F....FFFFFFFFFFFFFFFFFFFFF......│.....................FFFFFFFFFFFFFFF...│............F...........MMM│...................
FF^>│^>│.>│p^│<>│<FFFFFFFFFFFFFFFFF┌┘FFFFF...........└┐...│FFFFFFFFFFFFFFFFFFFFFFFFFFF......│.....................FFFFFFFFFFFFFFFFF.│..............┌┐........MMM│...................
FFFF│<>│<>├──┼─>│<FFFFFFFFFFXFFFF┌─┘FFFFFFFF..........│.┌─┘FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF..│.......................FFFFFFFFFFFFFFFF└┐.............+└┐.......MMM└┐..................
FFF>│<F│<>│C>│^.│<FFFFFFFFF┌┘FFFF│FFFFFFFFFF..........└┬┘.FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF..│....................F.FFFFFFFFFFFFFFFFFF│...............└──┐.......>│<.................
FFFO│<>│DO│.>│<>│<FFFFFFFF>│<FFFF│FFFFFFFFFF...........│..FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF..│.....................FFFFFFFFFFFFFFFFFFF│..................│......│>│<.................
FFFv│<A│SC│vv│<v│SFFF>│g..s│FFFFF│FFFFFFFFFF..........+│..FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF..└─┐...................FFFFFFFFFFFFFFFFFF.│..................│.>│<.wW>│<.................
FFF─┴──┼──┼──┼──┤vvvvO│gO.>│<vFFF│FFFFFFFFFF...F......F│F.FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF..##│##................FFFFFFFFFFFFFFFFFFF.│..................│v>│<gwwv│<v......FFF..FFF..
FFF^^^O│O>│O>│A>├─────┼────┼────┬┘FFFFFFFFFFF.FF.....FF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF..#>│##...............FFFFF+FFFFFFFFFFFFF..│..................└──┼──┼──┼─┐........FF┌+FF..
FFF....O.vv.g│<w│<s..#│^.g^│<.^.│FFFFFFFFFFFFFF..FF..FF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF##.##...........FFFFFFFFFF.FFFFFF...F....│...............F...^^│^>│ss│<│<..┌──────┘FFF..
FFFF.>┌───┐C>│v>│vDFgg│vvvv│vS..│FFFFFFFFFFFFF...FFFFFF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF...F##F##.....FFFFFFFFFFFFFFF...FFF..........│.....................vvF│v>│<│<.┌┘F....F......
FFFF..│hhO├──┼──┼─────┼────┼──F.│FFFFFFFF┌─BFF...FFFFFF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF....FFFFFFFFFFFFFFFFFFFFFFFFF...............─┤................FF..F───┼──┤.└┬─┘F.....FFF....
FFFF.>│hh>│<A│^>│<^F^>│AO^^│<^FF│FFFFFFF┌┤FFFF...FFFFFF└┐...FFFFFFFFFFFFFFFFFFFFF..FFFFFF...FFFFFFFFFFFFFFFFFFFFFFFFF..............ww│...............FFFF...^F│^^│<.│...F.....FFFF..
FHvvvO│.gv│<.vTT│vvvFO│<Ov>│..FF└┐FFFF┌─┴┤FFFF...FFF.FFF│...FFFFFFFFFFFFFFFFFFFF...FFFFFF..FFFFFFFFFFFFFFFFFF...FFFFF..............ww│......F........FFF.FFFF>│FFFF.└┐..F.....FFFFF.
FF────┼───┼───TT├─────┴────┘<....│FFFF│FF└┐FFF...FF...FF│.....FFFFFFFFFFFFFFFFF....FFFFFF..FFFFFFFFFFFFF..FFFFFFFF...FFFF............│......F.........FFFFFF.F.F....F│..F...FFFFFF..
FF.^.>│<p^│<.S.>│^^CM+MttO.^.....└─┐FF│FFF└┐FFF..FF...FF│......FFFFFFFFFFFFFFFFF......F.....FFFFFFFFFFFFF.FFFFFFFFF..FFFFF...........│......F..........┌───────┐.....└┐...FFFFFFFFFF
FFv.$>│vvv│vv.hh│vvvMMMttT.........│FF│FF┌─│FFF.FF....F.│.......FFFFFFFFFFFFFFF.............FFFFFFFFFFFFF...FFFFFFFFFFFFFF...........│.................│######P│......│...FFFFFFFFFF
F.────┼───┼───hh├───MMM............└──┘FF^FFFFFFFF......│.....SSSSSFFFFFFFFFFFFF.............FFFFFFFFFFFF...FFFFFFFFFFFFFF...........│.................│#######│......│...FFFFFFFFFF
..^^^>│^^>│<^^.>│^^FF>│<..............FFFFFF..FFFFF.....│.....SSSSSFFFFFFFFFFFF...............FFFFFFFFFFFF..FFFFFFFFFFFFFFF..........│.................│#######│......│....FF.FFFF..
.....v│vv>│vvS.>│<vvvv│vMvSvvvv.vv......FFF.............│.....SSSSS┐FFFFFFFFFF.................FFFFFFFFFFF..FFFFFFFFFFFFFFF..........└┐................│#######│......│.............
.....─┼───┼───┬─┼─────┼────┬──────......FFFFF....F......│.....||SSS│FFFFF.FFFF..................FFFFFFFFFFFFFFFFFFFFFFFFFF.........ppp│................│#######│......│.............
.....>│<.>│^.^│^│.###^│<..^│^^^^^^.....FF.FFFF...F......│.........┌┘FFFFF..FFFF..................FFFFFFFFFFFFFFFFFFFFFFFFFF........ppP┤................│#######│......│.............
..vv.>│Ov>│.vv│T│vv..>│<.vv│vv│<.......FFFFFFFFFF.......│.........│..FF....F.F...........%.......FFFFFFFFFFFFFFFFFFFFFFFFFFFF......ppp│........+.......│##Ov┐##│......│.............
..────┼───┼───┼─┴─────┼────┼──┘.......FFFFFFFFF.F.......│.........│...F.....................L.....FFFFFFFFFFFFFFFFFFFFFFFFFFF...┌─┐...│................└─┘O│└──┘┌─────┤.............
..^^^>│<^>│.^.│<^^^..^│^^^^│^.^......FFFFFFFFFF.........│.........│...F....................┌─.....FFFFFFFFFFFFFFFFFFFFFFFFFFFF┌─┘F│...│....................└────┘.....│.............
..vvvv│<av│sv.vv>│vvvv│###>│..vv....FF.F.FFFFFF......##.│.........│...F..F................┌┘.......FFFFFFFFFFFFFFFFFFFFFFFFFFF│FF.│...│..........FFF..................│.┌─nn........
..────┼───┼──────┼────┼────┼────...FFF...............##O│......┌──┴─┐FFFFFF┌──┬───┐.......│........FFFFFFFFFFFFFFFFFFFFFFFFFFF│FFF└───┤...F.....FFFFF.................├─┘.nn........
..^..^│^^^│^^^^.#│^^^^│^^^>│^^.^...FF.F..............##┤│.....┌┘FF..└──────┘#F│...└─┐.....│.........FFFFFFFFFFFFFFFFFFFFFFFFF┌┘FFFFFF.│.FFFF..FFFFFF..................│.............
..vvv>│<.>│.....#│<.vv│<...│O.v...FFFFFF...............─┤....┌┘FFFFFFFFFFFF┌┴─└┐....└─┐...│..........FFFFFFFFFFFFFFFFFFFFFFF┌┘FFFFFFFF│FFFFFF.FFFFFFFF................│.............
..────┘<.>│<....#├──┬─┴+..>└────....FFF.................│....│FFFFFFFFFFFFF^FFF└┐.....└─┐.│...........FFFFFFFMFFFFFFFFFFFFFF│FFFFFFFFF│FFF...F..FFFFFFF...............│.............
..^^............>│O^│<.......^.^....F...................│....│FFFFFFFFFFFFFFFFFF└┐......└─┤............FFFFFFFFFFFFFFFFFFFFF│FFFFFFFFF│FFF...FFFFFFFFFF.F...F.........│.............
................>│<.│...................................├────┘FFFFFFFFFFFFFFFFFFF│........│............FFFFFFFFFFFFFFFFFFFFF│FFFFFFFFF│FFF...FFFFFFFFFFFFFFFF.........│.............
F...............>│<v│vgv...........F....................│...F.FFFFFFFFFFFFFFFFFFF│........│............FFFFFFFFFFFFFFFFFF┌──┘FFFFFFFFF│FFFF..FFFFFFFFFFFFFFFF.........│.............
FF..............>└──┼───...........FF...................│..FFFFFFFF.FFFFFFFFFFFFF│FF.X....│FFFFFF......FFFFFFFFFFFFFFFFB┌┘FFFFFFFFFFF┌┘FFFF.FFFFFFFFFFFFFFFFFF........│.............
FFFFF.............^^│<^^...........FFF..................│.FFFFFFFFFFFFFFFFFFFFFFF│FFF.....│FFFFFFFFF...FFFF..FFFFFFFFFF└┘FFFFFFFFFFFF│SFFFFFFFFFFFFFFFFFFFFFFF........│.............
FFF................>│<.............FFFFF.F...F..F.......│..FFFFFFFFFFFFFFFFFFFFF┌┘F.F.....└┐F###oFFF..FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF####FFFFFFFFFF──┐......│.>│O........F
FF.................>│<...........FFFFFFFFFFFFFFFF.......│..FFFFFFFFFFFFFFFFFFFFF│FF........│F##&#FFF..FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF####FFFFFFFFFF0F│....nn│<.│..FF.....F
FFF............<...>│<...........FFFFFFFFFFFFFFF........│..FFFFFFFFFFFFFFFFFFFF┌┘FFF......┌┘Foo##FFF...FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF##v#FFFFFFFFFFF.└┐.>│nn│<.│aFFFFF....
F...................│...........FFFFFFFFFFFFFFFF........│.FFFFFFFFFFFFFFFFFF┌──┘FFFF......│FF^###FFFF..FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF┌─FFFFFFFFFFFF..│..│.o│<>│.FFFFF....
....................│........FFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFFFFFFFFFFFFFFF┌┘FFFFFFFF.....│FFF─┐FFFF..FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF┌┘FFFFFFFFFFFFFF.│.>│vv│xC│<vFv......
....................│........FFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFFFFFFFFFFFFFF─┘FFFFFFFFFF....│FFFF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF┌┘FFFFFFFFFFFF....└──┼──┼──┼────......
....................│.......FFFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFFFFFFFFFFFFFF├##FFFFFFFFFF...│FFFF│FFFFFFFFFFFFFFFFFFF.FFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFFFFFFFFFFF....^>│^>│O^│<.g^......
....................│........FFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFFFFFFFFFFFFFFO##FFFFFFFFFFF..└─┐FF│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF..FFFFFF│FFFFFFFFFFFFFF.O┌──Ttt>│.w│<.........
....................│......FFFFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFF......FFFFFFF##FFFFFFFFFF.....└──┤.FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF........FFFF│FFFFFFFFFFFFFFFC│sAOttJ│<>│<vv...FFFF
.......M............│......FFFFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFF......FFFFFFFFFFFFFFFFFFF........│.FFFFFFFFFFFFFFFFFF.F.FFFFFFFFFFFF.........FFF│FFFFFFFFFFFvvvFF│vF─┼──┼──┼──┬─FFFFFF
...................┌┘....FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFF.......FFFFFFFFFFFFFFFFFFF........│..FFFFFFFFFFFFFFFFF....FFF...F.FFF.......┌─┐FF│FFFFFFFFFFF─────┼──O│^L│^^│^>│<FFFFFF
...................│FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFF...F.FFFFFFFF.F..FFFFFFF....FFF..└─┐.FFFFFFFFFFFFF.........................│.└──┘FFFFFFFFFFF^^.^>│<RF│.L│.>│<>│OFFFFFF
...................│.FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFFFFF.FFFFFFFFF......FFFF.....FF.....└┐.FFFFFFFFFFF..........................│......FFFFFFFFFFF┌─┐>│pFv│<>│<S│<v│<FFFFFF
..........F........│..FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFHFF..FFFFFFFFF........F........FF....│.FFFFFFFFFFFF.........................│####..FFFFFFFFFF┌┘F└─┴───┼──┼──┼──┼─┐FFFFF
........F.F........│..FFFF.FFFFFFFFFFFFFFFFFFFFFFFFFFFFF│FFFFFFF..FFFFFFFFF.................FFFF..│.FFFFFFFFFFFFF...................───┐.│####..FFFFFFFFFF│FFFF^FF>│^^│g#│<>│<│<FFFF
...................│..FFF..FFFFFFFFFFFFFFFF...FFFFFFFFFF│FFFFFFFFFFFFFFFFFF................FFFFF..│..FFFFFFFFFFFFF.................#^##│.│─┴┬─FFFFFFFFF┌──┘FFFFFFF>│FF│<#│<>│<│<vvFF
...................│...FF..FFFFFFFFFFFFFFFFF..FFFFFFFFFF│.FFFFFFFFFFFFFFFFF...............FFFFFF..│.FFFFFFFFFFFFFFF................####└─┤FFOFFFFFF┌───┤FFFFFFFFFFF│<>│<#│.>│>├──┐FF
...................│......FFFFFFFFFFFFFFFFFF..FFFFFFFFFv│<.FFFFFFFFFFFFFFFF.FFF...........FFFFFF..│####.FFFFFFFFFFF................####..│FF.FF┌───┘FFF│FFFFFFFFFF>│<>│O>│.>│F│^^└─┐
...................│......FFFFFFFFFF.FFFFFFF....FFFFF..─┼─.FFFFFFFF.FFFFFFFFFFFF...........FFFFFF.│####..FFF.............................└──┬──┘FFFFFFF│FFFFFFFFFF>│<─┴──┘<P│<│<FF+┴
...................├────┐FFFFFFFF.##├OFFFFFF.....FFF...>│^.FFFFFFFFF..FFFFFFFFFFF..........FFFFFFF│##v#..FFF.A..............................│FFFFFFFFFF└┐FFFFFFFFFFFFO^^FF.P..FFFFFF
FF.................│....└┐FFFFFFFF##┤..FFFF................FFFFFF.FF...FFFFFFFFFF...........FFFFFF└───.F.FFF...............................┌┘FFFFFFFFFFX┘FFFFFFFFFFFFFFFF......FFFFF
FFF...........┌───┬┘.....└┐FFFFFFF##├<.FFFF................FFFFFF......FFFFFFFFFFF...........FFFFFF....F...FFFFF..........................┌┘FFFFFFFFFFFFFFFFFFFFFFFFFFFFF......FFFFF
FFF..........┌┤..XX.......│FFFFFFF##└┐F.FFF................FFFFF.....FFFF.FFFFFFFF............FFF......F...FFFF...........................│FFFFFFFFFFFFFFFFFFFFFFFFFFF..F.......FFFF
FFFF.........hh..XX.......│FFFF.FFFF.│FFFFF.........................FFFFFFFFFFFFFFF............F...........FFF..........................┌─┘FFFFFFFFFFFFFFFFFFFFFFFFFFF..F.......FF..
FFFFF.....................└──────────┐FFFF..........................FFFFFFFFFFFFFF.......................F.FF.......................┌───┘.FFF.FFFFFFFFFFFFFFFFFFFFFFFF..........F...
".Trim();
            var map = new Map();
            using (var reader = new StringReader(overmap))
            {
                var line = string.Empty;
                var y = 0;
                do
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        var x = 0;
                        foreach (var c in line)
                        {
                            map.Terrain[new Coordinate(x, y, 0)] = new Terrain { Renderable = new Renderable { Color = Color.WhiteSmoke, Symbol = c } };
                            x++;
                        }
                    }

                    y++;
                } while (line != null);
            }

            return map;
        }
    }
}
