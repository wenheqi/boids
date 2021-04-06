#version 330 core

attribute vec3 position;

// Values that stay constant for the whole mesh.
uniform mat4 MVP;

void main()
{
	gl_Position = MVP*vec4(position, 1.0);
}