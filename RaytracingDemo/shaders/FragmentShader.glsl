#version 330

uniform sampler2D tex;

in vec2 texCoord;
out vec4 Color;

void main() 
{
    Color = texture(tex, texCoord);
}
