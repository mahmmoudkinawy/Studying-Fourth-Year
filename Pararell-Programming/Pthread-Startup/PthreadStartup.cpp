#include <Windows.h>
#include <stdio.h>
#include <pthread.h>

int x;
pthread_mutex_t mut = PTHREAD_MUTEX_INITIALIZER;

void add_x(int a) {
    pthread_mutex_lock(&mut);
    x += a;
    pthread_mutex_unlock(&mut);
}

void* doit(void* arg) {
    for (int i = 0; i < 100; i++) {
        add_x(1);
    }
    printf("hello from another thread: %d\n", x);
    return NULL;
}

int main(int argc, char* argv[]) {
    for (int cur = 0; cur < 100; cur++) {
        pthread_t thread;
        pthread_create(&thread, NULL, *doit, NULL);
    }

    Sleep(300);
    printf("total: %d\n", x);
    return 0;
}