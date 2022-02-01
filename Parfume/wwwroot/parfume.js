var url = 'home/index';
self.addEventListener('push', function (event) {
    if (!(self.Notification && self.Notification.permission === 'granted')) {
        return;
    }

    var data = {};
    if (event.data) {
        console.log(event.data);
        data = { "message": "KenanHesenov Sabah odenisin vaxtidi", "title": "#1", "url": "Home/Pay?orderId=1" };
    }

    console.log('Notification Received:');
    console.log(data);

    var title = data.title;
    var message = data.message;
    url = data.url;
    var icon = "images/logo2.png";
    console.log(self);
    console.log(self.registration);
    event.waitUntil(self.registration.showNotification(title, {
        body: message,
        icon: icon,
        badge: icon,
        actions: [
            {
                action: 'view',
                title: 'Bax'
            }
        ],
        requireInteraction: true,
        data: data
    }));
});

self.addEventListener('notificationclick', function (event) {
    event.notification.close();
    url = event.notification.data.url;
    const promiseChain = clients.openWindow(url);
    event.waitUntil(promiseChain);
});