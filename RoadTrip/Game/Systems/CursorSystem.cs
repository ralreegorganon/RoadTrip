using System;
using System.Collections.Generic;
using System.Text;
using Leopotam.Ecs;
using RoadTrip.Game.Components;

namespace RoadTrip.Game.Systems
{
    public class CursorSystem : IEcsRunSystem
        {
        private EcsFilter<Position, CursorTag> CursorFilter { get; set; }
        private EcsFilter<Position, CameraFocusTag> CameraFocusFilter { get; set; }

        public void Run()
        {
            if (CursorFilter.IsEmpty() || CameraFocusFilter.IsEmpty()) {
                return;
            }

            ref var cursorEntity = ref CursorFilter.Entities[0];

            if (cursorEntity.Get<CameraFocusTag>() != null) {
                return;
            }

            ref var cameraFocusEntity = ref CameraFocusFilter.Entities[0];

            var cursorPos = cursorEntity.Get<Position>();
            var cameraPos = cameraFocusEntity.Get<Position>();

            cursorPos.Coordinate = cameraPos.Coordinate;
        }
    }
}
