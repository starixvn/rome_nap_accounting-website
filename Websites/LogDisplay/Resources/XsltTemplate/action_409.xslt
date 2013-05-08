<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>
  <xsl:template match="acc_customerupdmemberhr_mag">
    <table class="tblDesc">
      <xsl:for-each select="info">
        <tr class="headerDetailNormal">
          <td colspan="6">{lbl:custInfo}</td>
        </tr>
        <tr >
          <td class="colW1">{lbl:SubAccName}</td>
          <td class="colW2">
            <xsl:value-of select="@subaccname" />
          </td>
          <td class="colW1">{lbl:IP}</td>
          <td class="colW2">
            <xsl:value-of select="@ip" />
          </td>
          <td class="colW1">{lbl:Language}</td>
          <td class="colW2">
            <xsl:value-of select="@language" />
          </td>
        </tr>
        <tr >
          <td class="colW1">{lbl:ErrMsg}</td>
          <td class="colW2">
            <xsl:value-of select="@errmsg" />
          </td>
          <td class="colW1">{lbl:ErrCode}</td>
          <td class="colW2">
            <xsl:value-of select="@errcode" />
          </td>
          <td class="colW1"></td>
          <td class="colW2">

          </td>
        </tr>

      </xsl:for-each>
      
      <xsl:for-each select="m">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:m}</td>
        </tr>
        <tr >
          <td class="colW1">{lbl:v151mpt_31}</td>
          <td class="colW2">
            <xsl:value-of select="@v151mpt_31" />
          </td>
          <td class="colW1">{lbl:v151mpt_32}</td>
          <td class="colW2">
            <xsl:value-of select="@v151mpt_32" />
          </td>
          <td class="colW1">{lbl:v151mpt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@v151mpt_41" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:v151mpt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@v151mpt_42" />
          </td>
          <td class="colW1">{lbl:v151mpt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v151mpt_51" />
          </td>
          <td class="colW1">{lbl:v151mpt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v151mpt_52" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:v152mpt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@v152mpt_41" />
          </td>
          <td class="colW1">{lbl:v152mpt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@v152mpt_42" />
          </td>
          <td class="colW1">{lbl:v152mpt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v152mpt_51" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:v152mpt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v152mpt_52" />
          </td>
          <td class="colW1">{lbl:v153mpt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@v153mpt_51" />
          </td>
          <td class="colW1">{lbl:v153mpt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@v153mpt_52" />
          </td>
        </tr>

      </xsl:for-each>
      <xsl:for-each select="iam">
        <tr class="headerDetailNormal" >
          <td colspan="6">{lbl:iam}</td>
        </tr>
        <tr >
          <td class="colW1">{lbl:isa_v151mpt_31}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151mpt_31" />
          </td>
          <td class="colW1">{lbl:isa_v151mpt_32}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151mpt_32" />
          </td>
          <td class="colW1">{lbl:isa_v151mpt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151mpt_41" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:isa_v151mpt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151mpt_42" />
          </td>
          <td class="colW1">{lbl:isa_v151mpt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151mpt_51" />
          </td>
          <td class="colW1">{lbl:isa_v151mpt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151mpt_52" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:isa_v152mpt_41}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152mpt_41" />
          </td>
          <td class="colW1">{lbl:isa_v152mpt_42}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152mpt_42" />
          </td>
          <td class="colW1">{lbl:isa_v152mpt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152mpt_51" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:isa_v152mpt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152mpt_52" />
          </td>
          <td class="colW1">{lbl:isa_v153mpt_51}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v153mpt_51" />
          </td>
          <td class="colW1">{lbl:isa_v153mpt_52}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v153mpt_52" />
          </td>
        </tr>

      </xsl:for-each>
      
      <xsl:for-each select="iame">
        <tr class="headerDetailNormal">
          <td colspan="6">{lbl:iame}</td>
        </tr>
        <tr >
          <td class="colW1">{lbl:isa_v151mpt_mem_31}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151mpt_mem_31" />
          </td>
          <td class="colW1">{lbl:isa_v151mpt_mem_32}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151mpt_mem_32" />
          </td>
          <td class="colW1">{lbl:isa_v151mpt_mem_41}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151mpt_mem_41" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:isa_v151mpt_mem_42}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151mpt_mem_42" />
          </td>
          <td class="colW1">{lbl:isa_v151mpt_mem_51}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151mpt_mem_51" />
          </td>
          <td class="colW1">{lbl:isa_v151mpt_mem_52}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v151mpt_mem_52" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:isa_v152mpt_mem_41}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152mpt_mem_41" />
          </td>
          <td class="colW1">{lbl:isa_v152mpt_mem_42}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152mpt_mem_42" />
          </td>
          <td class="colW1">{lbl:isa_v152mpt_mem_51}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152mpt_mem_51" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:isa_v152mpt_mem_52}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v152mpt_mem_52" />
          </td>
          <td class="colW1">{lbl:isa_v153mpt_mem_51}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v153mpt_mem_51" />
          </td>
          <td class="colW1">{lbl:isa_v153mpt_mem_52}</td>
          <td class="colW2">
            <xsl:value-of select="@isa_v153mpt_mem_52" />
          </td>
        </tr>
      </xsl:for-each>
      <xsl:for-each select="aume">
        <tr class="headerDetailNormal">
          <td colspan="6">{lbl:aume}</td>
        </tr>
        <tr >
          <td class="colW1">{lbl:a_v151mpt_mem_31}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v151mpt_mem_31" />
          </td>
          <td class="colW1">{lbl:a_v151mpt_mem_32}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v151mpt_mem_32" />
          </td>
          <td class="colW1">{lbl:a_v151mpt_mem_41}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v151mpt_mem_41" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:a_v151mpt_mem_42}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v151mpt_mem_42" />
          </td>
          <td class="colW1">{lbl:a_v151mpt_mem_51}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v151mpt_mem_51" />
          </td>
          <td class="colW1">{lbl:a_v151mpt_mem_52}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v151mpt_mem_52" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:a_v152mpt_mem_41}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v152mpt_mem_41" />
          </td>
          <td class="colW1">{lbl:a_v152mpt_mem_42}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v152mpt_mem_42" />
          </td>
          <td class="colW1">{lbl:a_v152mpt_mem_51}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v152mpt_mem_51" />
          </td>
        </tr>
        <tr>
          <td class="colW1">{lbl:a_v152mpt_mem_52}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v152mpt_mem_52" />
          </td>
          <td class="colW1">{lbl:a_v153mpt_mem_51}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v153mpt_mem_51" />
          </td>
          <td class="colW1">{lbl:a_v153mpt_mem_52}</td>
          <td class="colW2">
            <xsl:value-of select="@a_v153mpt_mem_52" />
          </td>
        </tr>
      </xsl:for-each>

    </table>
  </xsl:template>
  <xsl:template match="acc_customerupdmember_positiontaking">
    <table class="tblDesc">
      <xsl:for-each select="info">
        <tr class="headerDetailNormal">
          <td colspan="6">{lbl:custInfo}</td>
        </tr>
        <tr >
          <td class="colW1">{lbl:updmultiple}</td>
          <td class="colW2">
            <xsl:value-of select="@updmultiple" />
          </td>
          <td class="colW1">{lbl:updpt}</td>
          <td class="colW2">
            <xsl:value-of select="@updpt" />
          </td>
          <td class="colW1">{lbl:updptv2}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv2" />
          </td>
        </tr>
        <tr >
          <td class="colW1">{lbl:updptv3}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv3" />
          </td>
          <td class="colW1">{lbl:updptv5}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv5" />
          </td>
          <td class="colW1">{lbl:updptv8}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv8" />
          </td>
        </tr>
        <tr >
          <td class="colW1">{lbl:updptv10}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv10" />
          </td>
          <td class="colW1">{lbl:updptv11}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv11" />
          </td>
          <td class="colW1">{lbl:updptv99}</td>
          <td class="colW2">
            <xsl:value-of select="@updptv99" />
          </td>
        </tr>
        <tr >

          <td class="colW1">{lbl:ip}</td>
          <td class="colW2">
            <xsl:value-of select="@ip" />
          </td>
          <td class="colW1">{lbl:language}</td>
          <td class="colW2">
            <xsl:value-of select="@language" />
          </td>
          <td class="colTitle"></td>
          <td class="colValue">

          </td>

        </tr>
        <tr >
          <td class="colW1">{lbl:subaccname}</td>
          <td class="colW2">
            <xsl:value-of select="@subaccname" />
          </td>
          <td class="colW1">{lbl:errcode}</td>
          <td class="colW2">
            <xsl:value-of select="@errcode" />
          </td>
          <td class="colW1">{lbl:errmsg}</td>
          <td class="colW2">
            <xsl:value-of select="@errmsg" />
          </td>
        </tr>

      </xsl:for-each>
      <xsl:for-each select="ag">
        <tr class="headerDetail" onclick="$('#ag{{tbodylogid}}').slideToggle();">
          <td>{lbl:ag}</td>
        </tr>
        <tr>
          <td>
            <div style="display:none;" id="ag{{tbodylogid}}">
              <table class="tblDesc" >
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:soccer}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_1" />
                  </td>
                  <td class="colTitle">{lbl:apt_2}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_2" />
                  </td>
                  <td class="colTitle">{lbl:apt_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:apt_4}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_4" />
                  </td>
                  <td class="colTitle">{lbl:apt_5}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_5" />
                  </td>
                  <td class="colTitle">{lbl:apt_6}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_6" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:apt_7}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_7" />
                  </td>
                  <td class="colTitle">{lbl:apt_8}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_8" />
                  </td>
                  <td class="colTitle">{lbl:apt_9}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_9" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:apt_10}</td>
                  <td class="colValue">
                    <xsl:value-of select="@apt_10" />
                  </td>
                  <td class="colTitle">{lbl:alivept_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@alivept_1" />
                  </td>
                  <td class="colTitle">{lbl:alivept_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@alivept_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:alivept_7}</td>
                  <td class="colValue">
                    <xsl:value-of select="@alivept_7" />
                  </td>
                  <td class="colTitle">{lbl:alivept_8}</td>
                  <td class="colValue">
                    <xsl:value-of select="@alivept_8" />
                  </td>
                  <td colspan="2"></td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:basketball}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v2apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_1" />
                  </td>
                  <td class="colTitle">{lbl:v2apt_2}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_2" />
                  </td>
                  <td class="colTitle">{lbl:v2apt_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v2apt_7}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_7" />
                  </td>
                  <td class="colTitle">{lbl:v2apt_8}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_8" />
                  </td>
                  <td class="colTitle">{lbl:v2apt_9}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_9" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v2apt_20}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v2apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:football}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v3apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v3apt_1" />
                  </td>
                  <td class="colTitle">{lbl:v3apt_2}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v3apt_2" />
                  </td>
                  <td class="colTitle">{lbl:v3apt_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v3apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v3apt_20}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v3apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:tennis}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v5apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v5apt_1" />
                  </td>
                  <td class="colTitle">{lbl:v5apt_2}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v5apt_2" />
                  </td>
                  <td class="colTitle">{lbl:v5apt_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v5apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v5apt_20}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v5apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:baseball}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v8apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v8apt_1" />
                  </td>
                  <td class="colTitle">{lbl:v8apt_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v8apt_3" />
                  </td>
                  <td class="colTitle">{lbl:v8apt_20}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v8apt_20" />
                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:golf}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v10apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v10apt_1" />
                  </td>
                  <td class="colTitle">{lbl:v10apt_2}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v10apt_2" />
                  </td>
                  <td class="colTitle">{lbl:v10apt_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v10apt_3" />
                  </td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v10apt_20}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v10apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:motorsports}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v11apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v11apt_1" />
                  </td>
                  <td class="colTitle">{lbl:v11apt_20}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v11apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
                <tr class="headerSportTypeDetail">
                  <td colspan="6">{lbl:othersports}</td>
                </tr>
                <tr >
                  <td class="colTitle">{lbl:v99apt_1}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v99apt_1" />
                  </td>
                  <td class="colTitle">{lbl:v99apt_2}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v99apt_2" />
                  </td>
                  <td class="colTitle">{lbl:v99apt_3}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v99apt_3" />
                  </td>
                </tr>
                <tr >

                  <td class="colTitle">{lbl:v99apt_20}</td>
                  <td class="colValue">
                    <xsl:value-of select="@v99apt_20" />
                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                  <td class="colTitle"></td>
                  <td class="colValue">

                  </td>
                </tr>
              </table>
            </div>
          </td>
        </tr>
      </xsl:for-each>

    </table>
  </xsl:template>

</xsl:stylesheet>
