uniform sampler2D texture;
uniform float rotation;

varying vec3 modelPos;

void main() {
    gl_FragColor = texture2D(texture, vec2(modelPos.x / 2.0 + rotation, modelPos.y));
}