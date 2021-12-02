class Hashtag群組 {
    目標容器;
    編輯模式 = true;
    標籤 = (hashtag) => `<a class="mr-1 btn ${this.#get類別風格(hashtag.類別)}" href="/Search?id=${hashtag.Id}">${hashtag.名稱}</a>`
    Input群組 = () => `
        <div class="input-group mb-3">
            <div class="input-group-prepend"></div>
            <input class="form-control" list="hashtags">
            <datalist id="hashtags">${this.#getHashtag選項(this.hashtags)}</datalist>
        </div>
        <input type="hidden" name="hashtags">`
    Input標籤 = (hashtag) => `<span class="mr-1 btn ${this.#get類別風格(hashtag.類別)}">${hashtag.名稱}</span>`
    Input選項 = (hashtag) => `<option hashtag="${hashtag.Id}" category="${hashtag.類別}" value="${hashtag.名稱}"></option>`
    類別選項 = (類別名稱) => `<option value="${類別名稱}"></option>`
    類別風格 = { "愛好": 'btn-warning', "地區": 'btn-info', "其他": 'btn-secondary' }
    HashtagModal = () => `
        <div class="modal fade" id="createHashtag" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">新增 Hashtag</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                        <label for="recipient-name" class="col-form-label">Hashtag 名稱:</label>
                        <input type="text" class="form-control">
                        <label for="recipient-name" class="col-form-label">Hashtag 類別:</label>
                        <select class="form-control">${this.#getHashtag類別()}</select>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">取消</button>
                        <button type="button" class="btn btn-primary" id="createHashtagSubmit">新增</button>
                    </div>
                </div>
            </div>
        </div>`

    新增Hashtag = (newHashtag) => {
        return new Promise((resolve) => {
            $.ajax({
                type: 'POST',
                url: '/Post/AddHashtag',
                data: newHashtag
            }).done((hashtag) => {
                console.log('完成ADD HASHTAG',hashtag);
                resolve(hashtag);
            });
        });
    }

    onchange() {
        this.目標容器.querySelector('input[type="hidden"]').value = this.value;
        console.log(this.value);
    }

    constructor(hashtags, 設定值) {
        this.hashtags = hashtags;
        Object.assign(this, 設定值);
    }

    get value() {
        var 選中 = this.目標容器.querySelectorAll('option[disabled="disabled"]');
        var result = [];
        for (var 選項 of 選中) {
            result.push(parseInt(選項.getAttribute('hashtag')));
        }
        return result;
    }

    at(queryString) {
        this.目標容器 = document.body.querySelector(queryString);
        var elements = this.編輯模式 ? this.#gen標籤選擇器() : this.#gen標籤群組()
        elements.forEach(element => this.目標容器.append(element))
    }

    #gen標籤選擇器() {
        var hashtags = this.hashtags;
        var 標籤選擇器 = convertToElementList(this.Input群組(hashtags) + this.HashtagModal());
        var input標籤群組 = queryInElementList('div.input-group-prepend', 標籤選擇器);
        var input輸入框 = queryInElementList('input[list="hashtags"]', 標籤選擇器);
        var input選項列表 = queryInElementList('datalist', 標籤選擇器);
        var 新hashtag名稱 = queryInElementList('div.modal-body input', 標籤選擇器);
        var 新hashtag類別 = queryInElementList('div.modal-body select', 標籤選擇器);
        var 新hashtagSubmit = queryInElementList('button#createHashtagSubmit', 標籤選擇器);

        var hashtag不存在 = () => {
            新hashtag名稱.value = input輸入框.value;
            jQuery.noConflict();
            $('#createHashtag').modal('show');
        };

        var hashtag存在 = () => {
            var 選中項 = queryInElementList(`option[value=${input輸入框.value}]`, 標籤選擇器);
            選中項.setAttribute('disabled', 'disabled');

            var 新標籤 = convertToElement(this.Input標籤({
                Id: 選中項.getAttribute('hashtag'),
                類別: 選中項.getAttribute('category'),
                名稱: 選中項.getAttribute('value')
            }));

            新標籤.onclick = () => {
                選中項.removeAttribute('disabled');
                新標籤.remove();
                this.onchange();
            };

            input標籤群組.append(新標籤);
            this.onchange();
        };

        input輸入框.onchange = () => {
            var hashtag名稱列表 = [];
            hashtags.forEach(hashtag => hashtag名稱列表.push(hashtag.名稱));

            hashtag名稱列表.includes(input輸入框.value) ? hashtag存在() : hashtag不存在();

            input輸入框.value = '';
        };

        新hashtagSubmit.onclick = () => {
            (async () => {
                var 新Hashtag = (await this.新增Hashtag({
                    名稱: 新hashtag名稱.value,
                    類別: 新hashtag類別.value
                }));

                hashtags.push(新Hashtag);
                input選項列表.append(convertToElement(this.Input選項(新Hashtag)));
                input輸入框.value = 新Hashtag.名稱;
                hashtag存在();

                $('#createHashtag').modal('hide');
                input輸入框.value = '';
            })()
        }

        return 標籤選擇器;
    }

    #getHashtag選項() {
        var Hashtag選項 = '';
        this.hashtags.forEach(hashtag => Hashtag選項 += this.Input選項(hashtag))
        return Hashtag選項;
    }

    #gen標籤群組() {
        var 標籤群組 = '';
        this.hashtags.forEach(hashtag => 標籤群組 += this.標籤(hashtag));
        return convertToElementList(標籤群組);
    }

    #getHashtag類別() {
        var Hashtag類別 = '';
        Object.keys(this.類別風格).forEach(類別名稱 => {
            Hashtag類別 += `<option>${類別名稱}</option>`;
        });
        return Hashtag類別;
    }

    #get類別風格(類別) {
        return this.類別風格[類別] ? this.類別風格[類別] : 'btn-secondary';
    }
}

function convertToElement(html) {
    var temporary = document.createElement('div');
    temporary.innerHTML = html;
    return temporary.firstElementChild;
}

function convertToElementList(html) {
    var temporary = document.createElement('div');
    temporary.innerHTML = html;
    var elementList = [];
    for (var element of temporary.children) {
        elementList.push(element);
    }
    return elementList;
}

function queryInElementList(query, elementList) {
    for (var element of elementList) {
        if (element.querySelector(query)) {
            return element.querySelector(query);
        }
    }
    return null;
}