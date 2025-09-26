using MirraGames.SDK.Common;
using UnityEngine;
using UnityEngine.UIElements;

namespace MirraGames.SDK.UIElements {

    public class GridViewport : VisualElement {

        private const string GridColorName = "Grid-Color";
        private static readonly Color GridColorDefault = new(0.13f, 0.13f, 0.13f);
        private static readonly Color BackgroundColorDefault = new(0.2f, 0.2f, 0.2f);

        private const int LineVertexCount = 4;
        private const int LineIndexCount = 6;

        private const float MinorLineFrequency = 10.0f;
        private const float MajorLineFrequency = 100.0f;

        private const float MinorLineThickness = 0.5f;
        private const float MajorLineThickness = 1.0f;

        private const float DefaultGridOffsetX = 50.0f;
        private const float DefaultGridOffsetY = 50.0f;

        public new class UxmlFactory : UxmlFactory<GridViewport, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits {

            private readonly UxmlColorAttributeDescription gridColor = new() {
                name = GridColorName,
                defaultValue = GridColorDefault
            };

            public override void Init(VisualElement element, IUxmlAttributes attributes, CreationContext context) {
                base.Init(element, attributes, context);
                GridViewport gridViewport = element as GridViewport;
                gridViewport.GridColor = gridColor.GetValueFromBag(attributes, context);
            }

        }

        private int pointerId = -1;
        private Vector2 pointerDownPosition = Vector2.zero;
        private Vector2 pointerDownOffset = Vector2.zero;
        private Vector2 contentOffset = Vector2.zero;

        public GridViewport() {
            VisualElement contentContainer = new() {
                name = Naming.USS.ContentContainer
            };
            hierarchy.Add(contentContainer);
            contentContainer.style.flexGrow = 1;
            style.flexGrow = 1;
            style.backgroundColor = BackgroundColorDefault;
            generateVisualContent += OnGenerateVisualContent;
            RegisterCallback<PointerDownEvent>(callback => {
                if (pointerId != -1) return;
                pointerId = callback.pointerId;
                pointerDownPosition = callback.position;
                pointerDownOffset = new(GridOffsetX, GridOffsetY);
                callback.StopPropagation();
            });
            RegisterCallback<PointerMoveEvent>(callback => {
                if (callback.pointerId != pointerId) return;
                Vector2 currentPosition = callback.position;
                Vector2 deltaPosition = currentPosition - pointerDownPosition;
                contentOffset = pointerDownOffset + deltaPosition;
                GridOffsetX = contentOffset.x;
                GridOffsetY = contentOffset.y;
                MarkDirtyRepaint();
                callback.StopPropagation();
            });
            RegisterCallback<PointerUpEvent>(callback => {
                if (callback.pointerId != pointerId) return;
                pointerId = -1;
                callback.StopPropagation();
            });
        }

        public override VisualElement contentContainer {
            get => this.Q<VisualElement>(Naming.USS.ContentContainer);
        }

        public Color GridColor { get; set; } = Color.black;

        public float GridOffsetX { get; set; } = DefaultGridOffsetX;
        public float GridOffsetY { get; set; } = DefaultGridOffsetY;

        public float InitialContentOffsetX { get; } = -DefaultGridOffsetX;
        public float InitialContentOffsetY { get; } = -DefaultGridOffsetY;

        public void Recalculate() {
            MarkDirtyRepaint();
        }

        private void OnGenerateVisualContent(MeshGenerationContext context) {
            Rect rect = contentRect;
            contentContainer.style.left = GridOffsetX + InitialContentOffsetX;
            contentContainer.style.top = GridOffsetY + InitialContentOffsetY;
            for (float y = GridOffsetY % MinorLineFrequency; y < contentRect.height; y += MinorLineFrequency) {
                if ((y - GridOffsetY) % MajorLineFrequency == 0 || y < 0) continue;
                DrawHorizontalLine(context, rect, y, MinorLineThickness);
            }
            for (float x = GridOffsetX % MinorLineFrequency; x < contentRect.width; x += MinorLineFrequency) {
                if ((x - GridOffsetX) % MajorLineFrequency == 0 || x < 0) continue;
                DrawVerticalLine(context, rect, x, MinorLineThickness);
            }
            for (float y = GridOffsetY % MajorLineFrequency; y < contentRect.height; y += MajorLineFrequency) {
                if (y < 0) continue;
                DrawHorizontalLine(context, rect, y, MajorLineThickness);
            }
            for (float x = GridOffsetX % MajorLineFrequency; x < contentRect.width; x += MajorLineFrequency) {
                if (x < 0) continue;
                DrawVerticalLine(context, rect, x, MajorLineThickness);
            }
        }

        private void DrawHorizontalLine(MeshGenerationContext context, Rect rect, float height, float thickness) {
            MeshWriteData meshWriteData = context.Allocate(LineVertexCount, LineIndexCount);
            Vector3 vertexA = new(rect.xMin, height - thickness, Vertex.nearZ);
            Vector3 vertexB = new(rect.xMax, height - thickness, Vertex.nearZ);
            Vector3 vertexC = new(rect.xMin, height + thickness, Vertex.nearZ);
            Vector3 vertexD = new(rect.xMax, height + thickness, Vertex.nearZ);
            meshWriteData.SetNextVertex(new Vertex() { position = vertexA, tint = GridColor });
            meshWriteData.SetNextVertex(new Vertex() { position = vertexB, tint = GridColor });
            meshWriteData.SetNextVertex(new Vertex() { position = vertexC, tint = GridColor });
            meshWriteData.SetNextVertex(new Vertex() { position = vertexD, tint = GridColor });
            meshWriteData.SetNextIndex(0);
            meshWriteData.SetNextIndex(1);
            meshWriteData.SetNextIndex(2);
            meshWriteData.SetNextIndex(1);
            meshWriteData.SetNextIndex(3);
            meshWriteData.SetNextIndex(2);
        }

        private void DrawVerticalLine(MeshGenerationContext context, Rect rect, float height, float thickness) {
            MeshWriteData meshWriteData = context.Allocate(LineVertexCount, LineIndexCount);
            Vector3 vertexA = new(height - thickness, rect.yMin, Vertex.nearZ);
            Vector3 vertexB = new(height + thickness, rect.yMin, Vertex.nearZ);
            Vector3 vertexC = new(height - thickness, rect.yMax, Vertex.nearZ);
            Vector3 vertexD = new(height + thickness, rect.yMax, Vertex.nearZ);
            meshWriteData.SetNextVertex(new Vertex() { position = vertexA, tint = GridColor });
            meshWriteData.SetNextVertex(new Vertex() { position = vertexB, tint = GridColor });
            meshWriteData.SetNextVertex(new Vertex() { position = vertexC, tint = GridColor });
            meshWriteData.SetNextVertex(new Vertex() { position = vertexD, tint = GridColor });
            meshWriteData.SetNextIndex(0);
            meshWriteData.SetNextIndex(1);
            meshWriteData.SetNextIndex(2);
            meshWriteData.SetNextIndex(1);
            meshWriteData.SetNextIndex(3);
            meshWriteData.SetNextIndex(2);
        }

    }

}